using Microsoft.EntityFrameworkCore;
using PW.DataAccess.ModelConfigurations;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PW.DataAccess
{
    public class PwDbContext : DbContext
    {
        public DbSet<PwTransaction> Transactions { get; set; }
        public DbSet<PwUser> Users { get; set; }
        
        public PwDbContext(DbContextOptions<PwDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PwUserConfig());
            modelBuilder.ApplyConfiguration(new PwTransactionConfig());
        }
    }
}
