using Marquesita.Infrastructure.Email;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailConfirmationAsync(Message message);
        Task SendRecoveryPasswordEmailAsync(Message message);
    }
}
