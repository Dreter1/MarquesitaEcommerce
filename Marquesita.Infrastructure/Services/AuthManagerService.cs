using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class AuthManagerService : IAuthManagerService
    {
        private readonly SignInManager<User> _signInManager;

        public AuthManagerService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(string username, string password)
        {
            return await _signInManager.PasswordSignInAsync(username, password, false, false);
        }

        public void LogOut()
        {
            _signInManager.SignOutAsync().Wait();
        }
    }
}
