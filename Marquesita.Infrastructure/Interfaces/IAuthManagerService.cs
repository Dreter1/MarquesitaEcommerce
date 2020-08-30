using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Interfaces
{
    public interface IAuthManagerService
    {
        Task<SignInResult> LoginAsync(string username, string password);
        void LogOut();
    }
}
