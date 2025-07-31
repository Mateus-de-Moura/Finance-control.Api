using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Azure.Core;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Entity;
using finance_control.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace finance_control.Infra.Data.Repositories
{
    public class UserRepository(FinanceControlContex context, IConvertFormFileToBytes convertFormFileToBytes) : IUserRepository
    {
        private readonly FinanceControlContex _context = context;
        private readonly IConvertFormFileToBytes _convertFormFileToBytes = convertFormFileToBytes;
        public async Task<Result<User>> Update(User user)
        {
            var existingUser = await _context.Users.Where(x => x.Id.Equals(user.Id)).FirstOrDefaultAsync();

            if (user == null)
                return Result.Error("Usuário nao foi localizado.");

            existingUser.Name = user.Name ?? existingUser.Name;
            existingUser.Surname = user.Surname ?? existingUser.Surname;
            existingUser.Email = user.Email ?? existingUser.Email;
            existingUser.UserName = user.UserName ?? existingUser.UserName;
            existingUser.Active = user.Active;
            existingUser.AppRoleId = user.AppRoleId != Guid.Empty ? user.AppRoleId : existingUser.AppRoleId;

            if (user.PhotosUsers != null)
                existingUser.PhotosUsers.PhotoUser = user.PhotosUsers.PhotoUser;

            _context.Users.Update(existingUser);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
                Result<User>.Success(existingUser) :
                Result.Error("Erro ao atualizar usuário");

        }
    }
}
