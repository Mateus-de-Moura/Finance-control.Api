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
    public class RevenuesConfiguration : IEntityTypeConfiguration<Revenues>
    {
        public void Configure(EntityTypeBuilder<Revenues> builder)
        {
            builder.ToTable(nameof(Revenues));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value)
                .HasColumnType("decimal(18,2)");


            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
