using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using SimpleEmailApp.Models;

namespace SimpleEmailApp.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(EmailDto request)
    {
        var email = new MimeMessage();

        var emailUserName = _configuration.GetSection("EmailUserName").Value;
        var emailPassword = _configuration.GetSection("EmailPassword").Value;
        var emailHost = _configuration.GetSection("EmailHost").Value;
            
        email.From.Add(MailboxAddress.Parse(emailUserName));
        email.To.Add(MailboxAddress.Parse(request.To));
        email.Subject = request.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = request.Body };
            
        using var smtp = new SmtpClient();
        smtp.Connect(emailHost, 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(emailUserName, emailPassword);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}