using Microsoft.EntityFrameworkCore;
using PW.DataAccess.Interfaces;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<PwUser, PwDbContext>, IUserRepository
    {
        public UserRepository(PwDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<PwUser> GetByEmailAsync(string email)
        {
            return await GetSingleAsync(x => x.Email == email);
        }

        public async Task<PwUser> GetByNameAsync(string name)
        {
            return await GetSingleAsync(x => x.UserName == name);
        }

        public async Task<PwUser> GetWithTransactionsByEmailAsync(string email)
        {            
            IQueryable<PwUser> query = _dbContext.Users;
            query = query.Include(u => u.PayeeTransactions).ThenInclude(t => t.Recipient);
            query = query.Include(u => u.RecipientTransactions).ThenInclude(t => t.Payee);
            return await query.Where(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}
