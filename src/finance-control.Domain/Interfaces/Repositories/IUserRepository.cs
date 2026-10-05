using Ardalis.Result;
using finance_control.Domain.Entity;

namespace finance_control.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<Result<User>> Create(User user);
        Task<Result<User>> Update(User user);
        Task<Result<User>> GetUserByEmailOrName(string emailOrName);
        Task<Result<User>> GetById(Guid id);
        Task<Result<User>> GetByEmail(string email);
        Task<Result<User>> GetByUserName(string userName);
        Task<Result<User>> GetByRefreshToken(string refreshToken);
        Task<Result<User>> GetForLogin(string login, bool matchEmail);
        IQueryable<User> GetUsersFilter();
        Task<List<AppRole>> GetRoles();

        /// <summary>Persiste alterações feitas em uma entidade User já rastreada (senha, refresh token, foto).</summary>
        Task<bool> SaveChanges(CancellationToken cancellationToken = default);
    }
}
