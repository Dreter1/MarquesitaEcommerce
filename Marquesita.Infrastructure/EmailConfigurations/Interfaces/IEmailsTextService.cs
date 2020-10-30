using Marquesita.Infrastructure.EmailConfigurations.Models;

namespace Marquesita.Infrastructure.EmailConfigurations.Interfaces
{
    public interface IEmailsTextService
    {
        string ConfirmMailTextEcommerce(Message message);
        string ConfirmMailTextShop(Message message);
        string RecoveryPasswordText(Message message);
        string SaleConfirmationText(Message message);
        string EcommerceSaleConfirmationText(Message message);
    }
}
