using Marquesita.Infrastructure.EmailConfigurations.Interfaces;
using Marquesita.Infrastructure.EmailConfigurations.Models;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Business;
using Marquesita.Models.Identity;
using System;
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

        public async Task GenerateAndSendSaleShopEmail(string userId, Sale sale)
        {
            var user = await _usersManager.GetUserByIdAsync(userId);
            var file = await _documentService.GeneratePdfSaleShop(sale);
            var message = new Message(new string[] { user.Email }, ConstantsService.EmailSubject.SALE_CLIENT_CONFIRMATION, user, null, null, file);
            await _emailSender.SendEmailSaleConfirmationAsync(message);
        }

        public async Task GenerateAndSendSaleEcommerceEmail(string userId, Sale sale)
        {
            var user = await _usersManager.GetUserByIdAsync(userId);
            var file = await _documentService.GeneratePdfSaleEcommerce(sale);
            var message = new Message(new string[] { user.Email }, ConstantsService.EmailSubject.SALE_CLIENT_CONFIRMATION, user, null, null, file);
            await _emailSender.SendEmailSaleConfirmationAsync(message);
        }


    }
}
