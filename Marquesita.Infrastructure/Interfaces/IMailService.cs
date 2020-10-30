using Marquesita.Models.Business;
using Marquesita.Models.Identity;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IMailService
    {
        Task GenerateAndSendConfirmationEmail(User user, string emailConfirmationLink);
        Task GenerateAndSendConfirmationEmailByShop(User user, string emailConfirmationLink, string forgotPasswordLink);
        Task GenerateAndSendResetPassword(User user, string resetPasswordLink);
        Task GenerateAndSendSaleShopEmail(string userId, Sale sale);
        Task GenerateAndSendSaleEcommerceEmail(string userId, Sale sale);
    }
}
