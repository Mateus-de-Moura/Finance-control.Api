using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BackgroundService.Common.Entity;

namespace BackgroundService.Services.Mail
{
    public  class MailSender 
    {        
        private readonly MailConfiguration _mailConfiguration;

        public MailSender(IConfiguration configuration)
        {
            _mailConfiguration = configuration
                .GetSection("MailConfiguration")
                .Get<MailConfiguration>()
                ?? throw new InvalidOperationException("MailConfiguration section is missing or invalid in appsettings.json"); ;
        }

        public  async Task Send(string email)
        {
            using var smtpClient = new SmtpClient(_mailConfiguration.Smtp)
            {
                Port = _mailConfiguration.Port,
                Credentials = new NetworkCredential(_mailConfiguration.Email, _mailConfiguration.Password),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true
            };

            var mailMesage = new MailMessage(_mailConfiguration.Email, email , "Assunto", "Conteúdo do e-mail")
            {
                IsBodyHtml = true,
            };

            try
            {
                await smtpClient.SendMailAsync(mailMesage);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao enviar e-mail.", ex);
            }
        }
    }
}
