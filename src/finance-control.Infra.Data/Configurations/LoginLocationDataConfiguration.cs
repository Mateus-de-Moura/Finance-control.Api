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
    public class LoginLocationDataConfiguration : IEntityTypeConfiguration<LoginLocationData>
    {
        public void Configure(EntityTypeBuilder<LoginLocationData> builder)
        {
            builder.ToTable(nameof(LoginLocationData));

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
