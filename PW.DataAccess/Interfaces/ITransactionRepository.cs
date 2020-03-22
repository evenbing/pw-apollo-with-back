using PW.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PW.DataAccess.Interfaces
{
    public interface ITransactionRepository : IBaseRepository<PwTransaction>
    {
        Task<IEnumerable<PwTransaction>> GetByEmailAsync(string email, int offset, int limit);
        Task<int> GetTotalCountByEmailAsync(string email);
    }
}
