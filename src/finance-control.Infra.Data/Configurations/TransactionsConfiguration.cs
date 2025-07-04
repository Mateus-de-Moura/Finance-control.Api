using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using finance_control.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finance_control.Infra.Data.Configurations
{
    public class TransactionsConfiguration : IEntityTypeConfiguration<Transactions>
    {
        public void Configure(EntityTypeBuilder<Transactions> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Value)
                .HasPrecision(18, 2);

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Transactions>(x => x.UserId);
        }
    }
}
