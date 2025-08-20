using System.Net;
using System.Net.Mail;
using WebApplication1.Application.Interfaces;

namespace WebApplication1.Application.Service;

public class EmailService(IConfiguration configuration) : IEmailService
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var client = new SmtpClient(configuration["SmtpSettings:Host"], int.Parse(configuration["SmtpSettings:Port"]!))
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(configuration["SmtpSettings:Username"], configuration["SmtpSettings:Password"])
        };

        return client.SendMailAsync(
            new MailMessage(from: configuration["SmtpSettings:Username"]!,
                to: email,
                subject,
                message
            ));
    }
}