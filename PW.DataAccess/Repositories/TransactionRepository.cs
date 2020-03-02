using PW.DataAccess.Interfaces;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PW.DataAccess.Repositories
{
    public class TransactionRepository : BaseRepository<PwTransaction, PwDbContext>, ITransactionRepository
    {
        public TransactionRepository(PwDbContext dbContext) : base(dbContext)
        {

        }
    }
}
