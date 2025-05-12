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
    public class PhotoUserConfiguration : IEntityTypeConfiguration<PhotosUsers>
    {
        public void Configure(EntityTypeBuilder<PhotosUsers> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                .WithOne(x => x.PhotosUsers)
                .IsRequired()
                 .HasForeignKey<PhotosUsers>(x => x.UserId);
        }
    }
}
