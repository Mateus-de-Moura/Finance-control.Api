using System.Text;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace BackgroundService.Auth
{
    public class BasicAuthAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _username = "admin";
        private readonly string _password = "password123"; 
        public bool Authorize([NotNull] DashboardContext context)
        {
            var request = context.GetHttpContext().Request;
       
            var authHeader = request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                var encodedCredentials = authHeader.Substring(6);
                var credentials = Encoding.ASCII.GetString(Convert.FromBase64String(encodedCredentials)).Split(':');
                var username = credentials[0];
                var password = credentials[1];
             
                return username == _username && password == _password;
            }
     
            return false;
        }
    }
}
