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
    public class NotifyConfiguration : IEntityTypeConfiguration<Notify>
    {
        public void Configure(EntityTypeBuilder<Notify> builder)
        {
            builder.ToTable(nameof(Notify));

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            builder.HasOne(x => x.Expenses)
                .WithMany()
                .HasForeignKey(x => x.ExpensesId);
        }
    }
}
