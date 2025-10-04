using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using BackgroundService.Common.Entity;

namespace BackgroundService.Services.Mail
{
    public class MailSender
    {
        private readonly MailConfiguration _mailConfiguration;

        public MailSender(IConfiguration configuration)
        {
            _mailConfiguration = configuration
                .GetSection("MailConfiguration")
                .Get<MailConfiguration>()
                ?? throw new InvalidOperationException("MailConfiguration section is missing or invalid in appsettings.json"); ;
        }

        public async Task Send()
        {
            var emailsToSend = new List<string>
            {
                "mateus.demoura@hotmail.com",
                //"silvaraphaela884@gmail.com"                
            };

            foreach (var email in emailsToSend)
            {
                using var smtpClient = new SmtpClient(_mailConfiguration.Smtp)
                {
                    Port = _mailConfiguration.Port,
                    Credentials = new NetworkCredential(_mailConfiguration.Email, _mailConfiguration.Password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                var mailMesage = new MailMessage(_mailConfiguration.Email, email, "FATURAS EM  ATRASO", GetBodyHtml())
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

        private string GetBodyHtml()
        {
            return "<!DOCTYPE html>\r\n<html lang=\"pt-BR\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            padding: 20px;\r\n            color: #333;\r\n        }\r\n        .container {\r\n            background-color: #fff;\r\n            padding: 30px;\r\n            border-radius: 8px;\r\n            box-shadow: 0 0 5px rgba(0,0,0,0.1);\r\n        }\r\n        .header {\r\n            font-size: 20px;\r\n            font-weight: bold;\r\n            color: #d9534f;\r\n            margin-bottom: 20px;\r\n        }\r\n        .message {\r\n            font-size: 16px;\r\n            line-height: 1.6;\r\n        }\r\n        .button {\r\n            display: inline-block;\r\n            margin-top: 20px;\r\n            padding: 12px 24px;\r\n            background-color: #0275d8;\r\n            color: white;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n        }\r\n        .footer {\r\n            margin-top: 30px;\r\n            font-size: 14px;\r\n            color: #777;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"header\">🔔 Aviso de Faturas em Atraso</div>\r\n        <div class=\"message\">\r\n            Prezado(a),<br><br>\r\n            Identificamos que você possui <strong>faturas em atraso</strong> no sistema.<br>\r\n            Para evitar cobranças adicionais, orientamos que acesse o sistema o quanto antes para regularizar sua situação.<br><br>\r\n            Clique no botão abaixo para acessar:\r\n            <br>\r\n            <a class=\"button\" href=\"http://localhost:5173/Notifica%C3%A7%C3%B5es\">Acessar o Sistema</a>\r\n        </div>\r\n        <div class=\"footer\">\r\n            Este é um e-mail automático. Por favor, não responda.\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";
        }
    }
}