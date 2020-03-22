using PW.DataTransferObjects.Transactions;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.Services.Interfaces
{
    public interface ITransactionService
    {
        Task CreateTransactionAsync(string payeeEmail, CreateTransactionDto createTransactionDto);
        Task<IEnumerable<TransactionDto>> GetTransactionsByEmailAsync(string email, int offset, int limit);
        Task<int> GetTotalCountByEmailAsync(string email);
    }
}
