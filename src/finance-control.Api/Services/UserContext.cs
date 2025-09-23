using finance_control.Api.Interfaces;

namespace finance_control.Api.Services
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {            
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                    throw new UnauthorizedAccessException("User ID not found in claims.");

                return Guid.Parse(userId);
            }
        }
    }
}
