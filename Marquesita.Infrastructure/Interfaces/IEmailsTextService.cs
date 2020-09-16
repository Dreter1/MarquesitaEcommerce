using Marquesita.Infrastructure.Email;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IEmailsTextService
    {
        string ConfirmMailText(Message message);
        string RecoveryPasswordText(Message message);
    }
}
