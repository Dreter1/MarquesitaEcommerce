using Marquesita.Infrastructure.EmailConfigurations.Interfaces;
using Marquesita.Infrastructure.EmailConfigurations.Models;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Identity;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class MailService: IMailService
    {
        private readonly IUserManagerService _usersManager;
        private readonly IEmailSenderService _emailSender;
        private readonly IDocumentsService _documentService;

        public MailService(IUserManagerService usersManager, IEmailSenderService emailSender, IDocumentsService documentService)
        {
            _usersManager = usersManager;
            _emailSender = emailSender;
            _documentService = documentService;
        }

        public async Task GenerateAndSendSaleEmail(string userId)
        {
            var user = await _usersManager.GetUserByIdAsync(userId);
            var file = _documentService.GeneratePdfSale();
            var message = new Message(new string[] { user.Email }, ConstantsService.EmailSubject.SALE_CLIENT_CONFIRMATION, user, null, null, file);
            await _emailSender.SendEmailSaleConfirmationAsync(message);
        }
        
        public async Task GenerateAndSendConfirmationEmail(User user, string emailConfirmationLink)
        {
            var message = new Message(new string[] { user.Email }, ConstantsService.EmailSubject.CONFIRM_EMAIL, user, emailConfirmationLink, null, null);
            await _emailSender.SendEmailConfirmationAsync(message);
        }          
        
        public async Task GenerateAndSendConfirmationEmailByShop(User user, string emailConfirmationLink, string forgotPasswordLink)
        {
            var message = new Message(new string[] { user.Email }, ConstantsService.EmailSubject.CONFIRM_EMAIL, user, emailConfirmationLink, forgotPasswordLink, null);
            await _emailSender.SendEmailConfirmationShopAsync(message);
        }        
        
        public async Task GenerateAndSendResetPassword(User user, string resetPasswordLink)
        {
            var message = new Message(new string[] { user.Email }, ConstantsService.EmailSubject.FORGOT_PASSWORD, user, resetPasswordLink, null, null);
            await _emailSender.SendRecoveryPasswordEmailAsync(message);
        }


    }
}
