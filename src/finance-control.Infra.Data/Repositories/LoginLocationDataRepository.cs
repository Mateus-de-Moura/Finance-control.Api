using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Azure.Core;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Infra.Data.Repositories
{
    public class LoginLocationDataRepository(FinanceControlContex context) : ILoginLocationDataRepository
    {
        private readonly FinanceControlContex _context = context;
        public async Task<Result<LoginLocationData>> Create(LoginLocationData loginLocationData)
        {
            if (loginLocationData == null)
                return Result<LoginLocationData>.Error();

            await _context.LoginLocationData.AddAsync(loginLocationData);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
                Result<LoginLocationData>.Success(loginLocationData) :
                Result<LoginLocationData>.Error("Erro ao salvar, tente novamente");
        }

        public async Task<Result<List<LoginLocationData>>> GetLocations(Guid UserId)
        {
            if(UserId == Guid.Empty)
                return Result<List<LoginLocationData>>.Error();

            var result = await _context.LoginLocationData.Where(x => x.UserId.Equals(UserId)).ToListAsync();

            return result != null ? 
                Result<List<LoginLocationData>>.Success(result) : 
                Result<List<LoginLocationData>>.Error();


        }
    }
}
