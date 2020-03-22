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
    public class TransactionRepository : BaseRepository<PwTransaction, PwDbContext>, ITransactionRepository
    {
        public TransactionRepository(PwDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<PwTransaction>> GetByEmailAsync(string email, int offset, int limit)
        {
            IQueryable<PwTransaction> query = _dbContext.Transactions;
            query = query.Include(t => t.Payee).Include(t => t.Recipient);
            query = query.Where(t => t.Payee.Email == email || t.Recipient.Email == email);
            query = query.OrderByDescending(t => t.TransactionDateTime);
            query = query.Skip(offset).Take(limit);                        
            return await query.ToListAsync();
        }

        public async Task<int> GetTotalCountByEmailAsync(string email)
        {
            IQueryable<PwTransaction> query = _dbContext.Transactions;
            query = query.Where(t => t.Payee.Email == email || t.Recipient.Email == email);                                    
            return await query.CountAsync();
        }
    }
}
