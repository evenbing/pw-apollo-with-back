using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PW.DataAccess.ModelConfigurations
{    
    internal class PwTransactionConfig : IEntityTypeConfiguration<PwTransaction>
    {
        public void Configure(EntityTypeBuilder<PwTransaction> entityBuilder)
        {
            entityBuilder.HasOne(t => t.Payee)
                .WithMany(u => u.PayeeTransactions);

            entityBuilder.HasOne(t => t.Recipient)
                .WithMany(u => u.RecipientTransactions);
        }
    }
}
