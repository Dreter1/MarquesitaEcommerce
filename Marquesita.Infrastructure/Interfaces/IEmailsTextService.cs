using Marquesita.Infrastructure.Email;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IEmailsTextService
    {
        string ConfirmMailTextEcommerce(Message message);
        string ConfirmMailTextShop(Message message);
        string RecoveryPasswordText(Message message);
    }
}
