using Marquesita.Models.Identity;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IMailService
    {
        Task GenerateAndSendSaleEmail(string userId);
        Task GenerateAndSendConfirmationEmail(User user, string emailConfirmationLink);
        Task GenerateAndSendConfirmationEmailByShop(User user, string emailConfirmationLink, string forgotPasswordLink);
        Task GenerateAndSendResetPassword(User user, string resetPasswordLink);
    }
}
