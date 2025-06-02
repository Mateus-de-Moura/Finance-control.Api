using finance_control.Domain.Enum;

namespace finance_control.Domain.Abstractions
{
    public interface IAuthService
    {
        public string GenerateJWT(string email, string username, Guid UserId);
        public string GenerateRefreshToken();
        Task<ValidationFieldsUserEnum> UniqueEmailAndUserName(string email, string username);
    }
}
