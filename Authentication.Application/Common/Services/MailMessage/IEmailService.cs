

namespace Authentication.Application.Common.Services.MailMessage;

public interface IEmailService
{
    Task SendEmailAsync(Message message);
}
