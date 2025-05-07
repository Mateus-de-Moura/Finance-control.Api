using finance_control.Domain.Abstractions;
using finance_control.Domain.Enum;
using finance_control.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace finance_control.Services.AuthService
{
    public class AuthService(IConfiguration configuration, FinanceControlContex context) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly FinanceControlContex _context = context;
        public string GenerateJWT(string email, string username)
        {
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var key = _configuration["JWT:key"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new("Email", email),
                new("Username",username),
                new("EmailIdentifier", email.Split("@").ToString()!),
                new("CurrenTime", DateTime.Now.ToString()),
                //new(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(issuer: issuer, audience: audience,
                claims: claims, expires: DateTime.Now.AddDays(7), signingCredentials: credentials);

            var tokenHendler = new JwtSecurityTokenHandler();

            return tokenHendler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var securityRandomBytes = new byte[128];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(securityRandomBytes);

            return Convert.ToBase64String(securityRandomBytes);
        }

        public async Task<ValidationFieldsUserEnum> UniqueEmailAndUserName(string email, string username)
        {
            var users = await _context.Users.ToListAsync();

            var userNameExists = users.Exists(x => x.UserName == username);
            var emailExists = users.Exists(x => x.Email == email);

            if (emailExists)
                return ValidationFieldsUserEnum.EmailUnavailable;
            else if (userNameExists)
                return ValidationFieldsUserEnum.UsernameUnavailable;
            else if (userNameExists && emailExists)
                return ValidationFieldsUserEnum.UsernameAndEmailUnavailable;
            else
                return ValidationFieldsUserEnum.isValid;
        }
    }
}
