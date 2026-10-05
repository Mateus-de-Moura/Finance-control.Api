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

        public async Task<Result<User>> GetUserByEmailOrName(string emailOrName)
        {
            var existingUser = await _context.Users
                .Include(x => x.PhotosUsers)
                .Where(x => x.Email!.Equals(emailOrName) 
                || x.Name!.Equals(emailOrName))
                .FirstOrDefaultAsync();

            return existingUser != null ? Result.Success(existingUser) : Result.Error();
        }

        public async Task<Result<User>> Create(User user)
        {
            await _context.Users.AddAsync(user);

            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected > 0 ?
                Result.Success(user) :
                Result.Error("Erro ao cadastrar usuário");
        }

        public async Task<Result<User>> GetById(Guid id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));

            return user != null ? Result.Success(user) : Result.NotFound();
        }

        public async Task<Result<User>> GetByEmail(string email)
        {
            var user = await _context.Users
                .Include(x => x.PhotosUsers)
                .FirstOrDefaultAsync(x => x.Email!.Equals(email));

            return user != null ? Result.Success(user) : Result.NotFound();
        }

        public async Task<Result<User>> GetByUserName(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);

            return user != null ? Result.Success(user) : Result.NotFound();
        }

        public async Task<Result<User>> GetByRefreshToken(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);

            return user != null ? Result.Success(user) : Result.NotFound();
        }

        public async Task<Result<User>> GetForLogin(string login, bool matchEmail)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .Include(x => x.PhotosUsers)
                .FirstOrDefaultAsync(x => x.UserName == login || (matchEmail && x.Email == login));

            return user != null ? Result.Success(user) : Result.NotFound();
        }

        public IQueryable<User> GetUsersFilter()
        {
            return _context.Users.AsNoTracking();
        }

        public async Task<List<AppRole>> GetRoles()
        {
            return await _context.AppRole.ToListAsync();
        }

        public async Task<bool> SaveChanges(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<Result<User>> Update(User user)
        {
            var existingUser = await _context.Users.Where(x => x.Id.Equals(user.Id)).FirstOrDefaultAsync();

            if (user == null)
                return Result.Error("Usuário nao foi localizado.");

            existingUser!.Name = user.Name ?? existingUser.Name;
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
