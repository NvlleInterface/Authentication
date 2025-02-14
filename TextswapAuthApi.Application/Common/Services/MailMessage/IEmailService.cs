

namespace TextswapAuthApi.Application.Common.Services.MailMessage;

public interface IEmailService
{
    Task SendEmailAsync(Message message);
}
