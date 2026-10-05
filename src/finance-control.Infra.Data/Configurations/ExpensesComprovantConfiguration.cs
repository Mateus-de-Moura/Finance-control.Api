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
    public class ExpensesComprovantConfiguration : IEntityTypeConfiguration<ExpensesComprovant>
    {
        public void Configure(EntityTypeBuilder<ExpensesComprovant> builder)
        {
            builder.ToTable("ExpensesComprovant");

            builder.HasKey(ec => ec.Id);
        }
    }
}
