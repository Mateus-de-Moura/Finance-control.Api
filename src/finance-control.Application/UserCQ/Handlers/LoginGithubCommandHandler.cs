using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using finance_control.Application.Response;
using finance_control.Application.UserCQ.Commands;
using finance_control.Application.UserCQ.ViewModels;
using finance_control.Domain.Abstractions;
using finance_control.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace finance_control.Application.UserCQ.Handlers
{
    internal class LoginGithubCommandHandler : IRequestHandler<LoginGithubCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;

        private readonly string _clientId;
        private readonly string _clientSecret;

        public LoginGithubCommandHandler(HttpClient httpClient, IAuthService authService, IMediator mediator, IUserRepository userRepository,
           IConfiguration configuration)
        {
            _httpClient = httpClient;
            _authService = authService;
            _mediator = mediator;
            _userRepository = userRepository;

            _clientId = configuration["GitHubAuth:ClientId"];
            _clientSecret = configuration["GitHubAuth:ClientSecret"];
        }

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(LoginGithubCommand request, CancellationToken cancellationToken)
        {
            var user = new RefreshTokenViewModel();
            string urlPhoto = string.Empty;

            // 1. Solicitar token do GitHub (sem alterações aqui, já está correto)
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token");
            tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            tokenRequest.Content = new StringContent(JsonSerializer.Serialize(new
            {
                client_id = _clientId,
                client_secret = _clientSecret,
                code = request.Code
            }), Encoding.UTF8, "application/json");

            var tokenResponse = await _httpClient.SendAsync(tokenRequest, cancellationToken);
         
            if (!tokenResponse.IsSuccessStatusCode)
            {
                var errorContent = await tokenResponse.Content.ReadAsStringAsync(cancellationToken);           
                return ResponseBase<RefreshTokenViewModel>.Fail("Error", $"Falha ao obter token do GitHub: {errorContent}", (int)tokenResponse.StatusCode);
            }

            var tokenContent = await tokenResponse.Content.ReadAsStringAsync(cancellationToken);
            var tokenData = JsonSerializer.Deserialize<Dictionary<string, object>>(tokenContent);

            if (tokenData == null || !tokenData.TryGetValue("access_token", out var accessTokenObj) || accessTokenObj.ToString() is null)
                return ResponseBase<RefreshTokenViewModel>.Fail("Error", "Não foi possível obter o access_token do GitHub.", 401);

            var accessToken = tokenData.GetValueOrDefault("access_token").ToString();   
        
            var userRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");

            // Adicione os cabeçalhos diretamente na requisição, não no HttpClient
            userRequest.Headers.Clear();
            userRequest.Headers.Add("User-Agent", "finance_control-app");
            userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


            var userResponse = await _httpClient.SendAsync(userRequest, cancellationToken);

            if (!userResponse.IsSuccessStatusCode)
            {                
                var errorContent = await userResponse.Content.ReadAsStringAsync(cancellationToken);
                return ResponseBase<RefreshTokenViewModel>.Fail("Error", $"Falha ao buscar usuário no GitHub: {errorContent}", (int)userResponse.StatusCode);
            }

            var userJson = await userResponse.Content.ReadAsStringAsync(cancellationToken);
            var userData = JsonSerializer.Deserialize<Dictionary<string, object>>(userJson);

            if (userData == null || !userData.TryGetValue("login", out var login))
                return ResponseBase<RefreshTokenViewModel>.Fail("Erro", "Usuário do GitHub não encontrado", 404);

            var email = userData.ContainsKey("email") && userData["email"] != null
                ? userData["email"].ToString()
                : string.Empty;

            urlPhoto = userData.ContainsKey("avatar_url") && userData["avatar_url"] != null 
                ? userData["avatar_url"].ToString()
                : string.Empty;

            if (string.IsNullOrEmpty(email))
            {
                var emailsRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
                emailsRequest.Headers.UserAgent.ParseAdd("finance_control");
                emailsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var emailsResponse = await _httpClient.SendAsync(emailsRequest, cancellationToken);
                if (emailsResponse.IsSuccessStatusCode)
                {
                    var emailsJson = await emailsResponse.Content.ReadAsStringAsync(cancellationToken);
                    var emailsData = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(emailsJson);
                
                    var primaryEmailObject = emailsData?.FirstOrDefault(e =>
                        e.TryGetValue("primary", out var primaryElement) && primaryElement.ValueKind == JsonValueKind.True &&
                        e.TryGetValue("verified", out var verifiedElement) && verifiedElement.ValueKind == JsonValueKind.True
                    );

                    if (primaryEmailObject != null && primaryEmailObject.TryGetValue("email", out var primaryEmailElement))                    
                        email = primaryEmailElement.GetString();                    
                                 
                }
            }

            if (string.IsNullOrEmpty(email)) { return ResponseBase<RefreshTokenViewModel>.Fail("Erro", "Não foi possível obter um email primário do GitHub.", 400); };

            var result = await _userRepository.GetUserByEmailOrName(email);

            if (result.IsSuccess)
            {
                var resultAuth = await _mediator.Send(new LoginUserCommand { Email = email, Password = "user-github" });
                if (resultAuth.ResponseInfo is not null)
                    return ResponseBase<RefreshTokenViewModel>.Fail("Erro", "Erro ao fazer login", 401);

                user = resultAuth.Value;
            }
            else
            {
                var resultCreated = await _mediator.Send(new CreateUserCommand
                {
                    Active = true,
                    Name = login.ToString(),
                    Email = email,
                    Password = "user-github",
                    Username = login.ToString(),
                    RoleId = new Guid("f39b093c-9887-4a86-bba5-48be3c1466e4")
                });

                if (resultCreated.ResponseInfo is not null)
                    return ResponseBase<RefreshTokenViewModel>.Fail("Erro", "Erro ao fazer login", 401);

                var resultAuth = await _mediator.Send(new LoginUserCommand { Email = email, Password = "user-github" });
                if (resultAuth.ResponseInfo is not null)
                    return ResponseBase<RefreshTokenViewModel>.Fail("Erro", "Erro ao fazer login", 401);

                user = resultAuth.Value;
            }     
            
            user.Photo = urlPhoto;

            return ResponseBase<RefreshTokenViewModel>.Success(user);
        }
    }
}
