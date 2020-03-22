using Microsoft.AspNetCore.SignalR;
using PW.DataAccess.Interfaces;
using PW.DataTransferObjects.Transactions;
using PW.Entities;
using PW.Services.Exceptions;
using PW.Services.Hubs;
using PW.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        private IHubContext<BalanceHub, IBalanceHubClient> _balanceHubContext;
        
        public TransactionService(ITransactionRepository transactionRepository, 
            IUserRepository userRepository,             
            IHubContext<BalanceHub, IBalanceHubClient> balanceHubContext)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;            
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

        public async Task<IEnumerable<TransactionDto>> GetTransactionsByEmailAsync(string email, int offset, int limit)
        {
            var transactions = await _transactionRepository.GetByEmailAsync(email, offset, limit);
            var result = new List<TransactionDto>();
            foreach (var transaction in transactions)
            {
                if (transaction.Payee.Email == email)
                {
                    var transactionDto = new TransactionDto
                    {
                        Date = transaction.TransactionDateTime.ToString("u", new CultureInfo("en-US")),
                        CorrespondentName = transaction.Recipient.UserName,
                        Amount = -transaction.Amount,
                        ResultBalance = transaction.ResultingPayeeBalance
                    };
                    result.Add(transactionDto);
                }
                else
                {
                    var transactionDto = new TransactionDto
                    {
                        Date = transaction.TransactionDateTime.ToString("u", new CultureInfo("en-US")),
                        CorrespondentName = transaction.Payee.UserName,
                        Amount = transaction.Amount,
                        ResultBalance = transaction.ResultingRecipientBalance
                    };
                    result.Add(transactionDto);
                }
            }
            
            return result;
        }

        public async Task<int> GetTotalCountByEmailAsync(string email)
        {
            var result = await _transactionRepository.GetTotalCountByEmailAsync(email);
            return result;
        }
    }
}
