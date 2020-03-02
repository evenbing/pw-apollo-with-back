using PW.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PW.DataAccess.Interfaces
{
    public interface IUserRepository : IBaseRepository<PwUser>
    {
        Task<PwUser> GetByEmailAsync(string email);
        Task<PwUser> GetByNameAsync(string name);
        Task<PwUser> GetWithTransactionsByEmailAsync(string email);
    }
}
