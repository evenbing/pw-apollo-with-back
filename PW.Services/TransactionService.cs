using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using PW.DataAccess.Interfaces;
using PW.DataTransferObjects.Transactions;
using PW.Entities;
using PW.Services.Exceptions;
using PW.Services.Hubs;
using PW.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Services
{
    public class TransactionService : ITransactionService
    {
        private const string TransactionSizeErrorMessage = "Transaction size exceeds the current balance";
        private const string SendSelfErrorMessage = "PW cannot be sent to yourself";
        private const string UserNotExistErrorMessage = "User with name \"{0}\" does not exist";

        private ITransactionRepository _transactionRepository;
        private IUserRepository _userRepository;
        private IMapper _mapper;
        private IHubContext<BalanceHub, IBalanceHubClient> _balanceHubContext;
        
        public TransactionService(ITransactionRepository transactionRepository, 
            IUserRepository userRepository, 
            IMapper mapper,
            IHubContext<BalanceHub, IBalanceHubClient> balanceHubContext)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _balanceHubContext = balanceHubContext;
        }

        public async Task CreateTransactionAsync(string payeeEmail, CreateTransactionDto createTransactionDto)
        {
            var payee = await _userRepository.GetByEmailAsync(payeeEmail);
            var recipient = await _userRepository.GetByNameAsync(createTransactionDto.Recipient);

            ValidateCreation(payee, recipient, createTransactionDto.Amount, createTransactionDto.Recipient);

            payee.Balance -= createTransactionDto.Amount;
            recipient.Balance += createTransactionDto.Amount;

            var transaction = new PwTransaction
            {
                Payee = payee,
                Recipient = recipient,
                ResultingPayeeBalance = payee.Balance,
                ResultingRecipientBalance = recipient.Balance,
                Amount = createTransactionDto.Amount,
                TransactionDateTime = DateTime.Now
            };

            await _transactionRepository.AddAsync(transaction);            
            await _balanceHubContext.Clients.Group(recipient.Email).UpdateBalance(recipient.Balance);
        }

        private void ValidateCreation(PwUser payee, PwUser recipient, int amount, string recipientName)
        {
            if (recipient == null)
            {
                throw new PWException(string.Format(UserNotExistErrorMessage, recipientName));
            }

            if (amount > payee.Balance)
            {
                throw new PWException(TransactionSizeErrorMessage);
            }

            if (payee.Id == recipient.Id)
            {
                throw new PWException(SendSelfErrorMessage);
            }
        }

        public async Task<IOrderedEnumerable<TransactionDto>> GetTransactionsOrderedByDateAsync(string email)
        {
            var result = (await GetTransactionsAsync(email)).OrderByDescending(t => t.Date);
            return result;
        }

        private async Task<IEnumerable<TransactionDto>> GetTransactionsAsync(string email)
        {
            var user = await _userRepository.GetWithTransactionsByEmailAsync(email);
            var payeeDtos = user.PayeeTransactions.Select(t => {
                var transactionDto = _mapper.Map<TransactionDto>(t);
                transactionDto.CorrespondentName = t.Recipient.UserName;
                transactionDto.Amount = -t.Amount;
                transactionDto.ResultBalance = t.ResultingPayeeBalance;
                return transactionDto;
            });
            var recipientDtos = user.RecipientTransactions.Select(t => {
                var transactionDto = _mapper.Map<TransactionDto>(t);
                transactionDto.CorrespondentName = t.Payee.UserName;
                transactionDto.Amount = t.Amount;
                transactionDto.ResultBalance = t.ResultingRecipientBalance;
                return transactionDto;
            });

            var result = payeeDtos.Union(recipientDtos);
            return result;
        }        
    }
}
