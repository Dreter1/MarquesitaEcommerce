using Marquesita.Infrastructure.Email;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}
