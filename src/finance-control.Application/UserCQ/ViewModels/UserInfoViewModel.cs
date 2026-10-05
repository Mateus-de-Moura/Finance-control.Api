namespace finance_control.Application.UserCQ.ViewModels
{
    /// <summary>
    /// Dados do usuário devolvidos ao front. Não inclui tokens: JWT e refresh token
    /// trafegam apenas em cookies HttpOnly.
    /// </summary>
    public record UserInfoViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string Photo {  get; set; }
    }
}
