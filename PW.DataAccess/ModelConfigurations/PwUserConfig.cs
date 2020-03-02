using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PW.DataAccess.ModelConfigurations
{    
    internal class PwUserConfig : IEntityTypeConfiguration<PwUser>
    {
        public void Configure(EntityTypeBuilder<PwUser> entityBuilder)
        {
            entityBuilder.HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
