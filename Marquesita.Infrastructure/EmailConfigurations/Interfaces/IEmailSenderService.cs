using Marquesita.Infrastructure.EmailConfigurations.Models;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.EmailConfigurations.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailConfirmationAsync(Message message);
        Task SendEmailConfirmationShopAsync(Message message);
        Task SendRecoveryPasswordEmailAsync(Message message);
        Task SendEmailSaleConfirmationAsync(Message message);
        Task SendEmailEcommerceSaleConfirmationAsync(Message message);
    }
}
