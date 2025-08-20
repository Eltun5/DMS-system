namespace WebApplication1.Application.Interfaces;

public interface IEmailService
{
    public Task SendEmailAsync(string to, string subject, string message);
}