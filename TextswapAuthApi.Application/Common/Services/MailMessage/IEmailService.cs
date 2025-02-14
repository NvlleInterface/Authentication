

namespace TextswapAuthApi.Application.Common.Services.MailMessage;

public interface IEmailService
{
    void SendEmail(Message message);
}
