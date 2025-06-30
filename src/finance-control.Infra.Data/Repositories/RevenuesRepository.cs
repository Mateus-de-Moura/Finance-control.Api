using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Azure;
using Azure.Core;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Infra.Data.Repositories
{
    public class RevenuesRepository(FinanceControlContex context) : IRevenuesRepository
    {
        private readonly FinanceControlContex _context = context;
        public async Task<Result<Revenues>> CreateRevenue(Revenues revenues)
        {
            if (revenues is null)
                Result.Error("Preencha todos os campos obrigatórios.");

            var result = await _context.Revenues.AddAsync(revenues);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
                Result.Success(revenues) :
                Result.Error();
        }

        public async Task<Result<Revenues>> GetRevenuesById(Guid RevenuesId)
        {
            var revenue = await _context.Revenues
               .Where(x => x.Id.Equals(RevenuesId))
               .FirstOrDefaultAsync();

            return revenue != null ? 
                Result.Success(revenue) :
                Result.Error();
        }

        public async Task<Result<Revenues>> UpdateRevenue(Revenues revenues)
        {
            var revenue = await _context.Revenues.Where(x => x.Id.Equals(revenues.Id)).FirstOrDefaultAsync();

            if (revenue == null)
                Result.Error("Revenue não localizado");

            revenue.Active = revenues.Active;
            revenue.IsRecurrent = revenues.IsRecurrent;
            revenue.Description = revenues.Description;
            revenue.Value = revenues.Value;
            revenue.Date = revenues.Date;
            revenue.CategoryId = revenues.CategoryId;

            _context.Entry(revenue).State = EntityState.Modified;

           var rowsAffected =  await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
               Result.Success(revenue) :
               Result.Error();
        }
    }
}
