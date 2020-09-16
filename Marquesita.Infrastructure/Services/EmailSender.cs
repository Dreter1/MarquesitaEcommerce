using MailKit.Net.Smtp;
using Marquesita.Infrastructure.Email;
using Marquesita.Infrastructure.Interfaces;
using MimeKit;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IEmailsTextService _emailText;

        public EmailSender(EmailConfiguration emailConfig, IEmailsTextService emailText)
        {
            _emailConfig = emailConfig;
            _emailText = emailText;
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = string.Format(@"
<table width='100%' cellspacing='0' cellpadding='0' border='0' name='bmeMainBody'
    style='background-color:rgb(202,220,202)' bgcolor='#cadcca'>
    <tbody>
        <tr>
            <td width='100%' valign='top' align='center'>
                <table cellspacing='0' cellpadding='0' border='0' name='bmeMainColumnParentTable'>
                    <tbody>
                        <tr>
                            <td name='bmeMainColumnParent'
                                style='border:0px none transparent;border-radius:0px;border-collapse:separate'>
                                <table name='bmeMainColumn'
                                    class='m_3095773769270076491bmeHolder m_3095773769270076491bmeMainColumn'
                                    style='max-width:600px;overflow:visible;border-radius:0px;border-collapse:separate;border-spacing:0px'
                                    cellspacing='0' cellpadding='0' border='0' align='center'>
                                    <tbody>
                                        <tr>
                                            <td width='100%'
                                                class='m_3095773769270076491blk_container m_3095773769270076491bmeHolder'
                                                name='bmePreHeader' valign='top' align='center'
                                                style='color:rgb(102,102,102);border:0px none transparent' bgcolor=''>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width='100%' class='m_3095773769270076491bmeHolder' valign='top'
                                                align='center' name='bmeMainContentParent'
                                                style='border:0px none rgb(102,102,102);border-radius:0px;border-collapse:separate;border-spacing:0px;overflow:hidden'>
                                                <table name='bmeMainContent'
                                                    style='border-radius:0px;border-collapse:separate;border-spacing:0px;border:0px none transparent'
                                                    width='100%' cellspacing='0' cellpadding='0' border='0'
                                                    align='center'>
                                                    <tbody>
                                                        <tr>
                                                            <td width='100%'
                                                                class='m_3095773769270076491blk_container m_3095773769270076491bmeHolder'
                                                                name='bmeHeader' valign='top' align='center'
                                                                style='color:rgb(56,56,56);border:0px none transparent;background-color:rgb(43,43,43)'
                                                                bgcolor='#2b2b2b'>
                                                                <div id='m_3095773769270076491dv_7'>
                                                                    <table width='600' cellspacing='0' cellpadding='0'
                                                                        border='0' class='m_3095773769270076491blk'
                                                                        name='blk_divider'>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td
                                                                                    style='padding-top:20px;padding-bottom:20px;padding-left:20px;padding-right:20px'>
                                                                                    <table
                                                                                        class='m_3095773769270076491tblLine'
                                                                                        cellspacing='0' cellpadding='0'
                                                                                        border='0' width='100%'
                                                                                        style='border-top-width:0px;border-top-style:none;min-width:1px'>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td><span></span></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div id='m_3095773769270076491dv_9'>
                                                                    <table width='600' cellspacing='0' cellpadding='0'
                                                                        border='0' class='m_3095773769270076491blk'
                                                                        name='blk_text'>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <table cellpadding='0'
                                                                                        cellspacing='0' border='0'
                                                                                        width='100%'>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td class='m_3095773769270076491tdPart'
                                                                                                    valign='top'
                                                                                                    align='center'>
                                                                                                    <table
                                                                                                        cellspacing='0'
                                                                                                        cellpadding='0'
                                                                                                        border='0'
                                                                                                        width='600'
                                                                                                        name='tblText'
                                                                                                        style='float:left;background-color:transparent'
                                                                                                        align='left'
                                                                                                        class='m_3095773769270076491tblText'>
                                                                                                        <tbody>
                                                                                                            <tr>
                                                                                                                <td valign='top'
                                                                                                                    align='left'
                                                                                                                    name='tblCell'
                                                                                                                    style='padding:10px 20px;font-family:Arial,Helvetica,sans-serif;font-size:14px;font-weight:400;color:rgb(56,56,56);text-align:left;word-break:break-word'
                                                                                                                    class='m_3095773769270076491tblCell'>
                                                                                                                    <div
                                                                                                                        style='line-height:200%;text-align:center'>
                                                                                                                        <span
                                                                                                                            style='color:#ffffff;font-family:Helvetica,Arial,sans-serif'><span
                                                                                                                                style='font-size:30px'><strong>ACTIVE SU CORREO ELECTRONICO</strong></span></span>
                                                                                                                        <br><span
                                                                                                                            style='font-size:14px;font-family:Helvetica,Arial,sans-serif;color:#ffffff;line-height:200%'>Active su cuenta para poder hacer compras por La Marquesita</span>
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </tbody>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div id='m_3095773769270076491dv_13'>
                                                                    <table width='600' cellspacing='0' cellpadding='0'
                                                                        border='0' class='m_3095773769270076491blk'
                                                                        name='blk_divider'>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td
                                                                                    style='padding-top:20px;padding-bottom:20px;padding-left:20px;padding-right:20px'>
                                                                                    <table
                                                                                        class='m_3095773769270076491tblLine'
                                                                                        cellspacing='0' cellpadding='0'
                                                                                        border='0' width='100%'
                                                                                        style='border-top-width:0px;border-top-style:none;min-width:1px'>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td><span></span></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div id='m_3095773769270076491dv_14'>
                                                                    <table width='600' cellspacing='0' cellpadding='0'
                                                                        border='0' class='m_3095773769270076491blk'
                                                                        name='blk_divider'
                                                                        style='background-color:rgb(255,255,255)'>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td style='padding:20px'>
                                                                                    <table
                                                                                        class='m_3095773769270076491tblLine'
                                                                                        cellspacing='0' cellpadding='0'
                                                                                        border='0' width='100%'
                                                                                        style='border-top-width:0px;border-top-style:none;min-width:1px'>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td><span></span></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div id='m_3095773769270076491dv_3'>
                                                                    <table width='600' cellspacing='0' cellpadding='0'
                                                                        border='0' class='m_3095773769270076491blk'
                                                                        name='blk_text'>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td>
                                                                                    <table cellpadding='0'
                                                                                        cellspacing='0' border='0'
                                                                                        width='100%'>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td class='m_3095773769270076491tdPart'
                                                                                                    valign='top'
                                                                                                    align='center'
                                                                                                    style='background-color:rgb(255,255,255)'>
                                                                                                    <table
                                                                                                        cellspacing='0'
                                                                                                        cellpadding='0'
                                                                                                        border='0'
                                                                                                        width='600'
                                                                                                        name='tblText'
                                                                                                        style='float:left;background-color:rgb(255,255,255)'
                                                                                                        align='left'
                                                                                                        class='m_3095773769270076491tblText'>
                                                                                                        <tbody>
                                                                                                            <tr>
                                                                                                                <td valign='top'
                                                                                                                    align='left'
                                                                                                                    name='tblCell'
                                                                                                                    style='padding:20px;font-family:Arial,Helvetica,sans-serif;font-size:14px;font-weight:400;color:rgb(56,56,56);text-align:left;word-break:break-word'
                                                                                                                    class='m_3095773769270076491tblCell'>
                                                                                                                    <div
                                                                                                                        style='line-height:200%'>
                                                                                                                        <span>Estimado, le damos la bienvenida a La Marquesita</span>
                                                                                                                        <br><span style='font-family:Helvetica,Arial,sans-serif;font-size:14px;color:#808080;line-height:200%'>
                                                                                                                            para que pueda hacer comprar y demas porfavor verifica su cuenta haciendo click en el boton de abajo
                                                                                                                        </span>
                                                                                                                    </div>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </tbody>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width='100%'
                                                                class='m_3095773769270076491blk_container m_3095773769270076491bmeHolder m_3095773769270076491bmeBody'
                                                                name='bmeBody' valign='top' align='center'
                                                                style='color:rgb(226,226,226);border:0px none transparent;background-color:rgb(226,226,226)'
                                                                bgcolor='#e2e2e2'>
                                                                <div id='m_3095773769270076491dv_4'>
                                                                    <table width='600' cellspacing='0' cellpadding='0'
                                                                        border='0' class='m_3095773769270076491blk'
                                                                        name='blk_button'>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td width='20'></td>
                                                                                <td align='center'>
                                                                                    <table
                                                                                        class='m_3095773769270076491tblContainer'
                                                                                        cellspacing='0' cellpadding='0'
                                                                                        border='0' width='100%'>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td height='10'></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align='left'>
                                                                                                    <table
                                                                                                        cellspacing='0'
                                                                                                        cellpadding='0'
                                                                                                        border='0'
                                                                                                        width='100%'
                                                                                                        style='border-collapse:separate'>
                                                                                                        <tbody>
                                                                                                            <tr>
                                                                                                                <td
                                                                                                                    style='border-radius:0px;border:0px none transparent;text-align:center;font-family:Arial,Helvetica,sans-serif;font-size:14px;padding:20px 40px;font-weight:bold;background-color:rgb(97,166,95)'>
                                                                                                                    <span
                                                                                                                        style='font-family:Helvetica,Arial,sans-serif;font-size:18px;color:rgb(255,255,255)'><a
                                                                                                                            style='color:#ffffff;text-decoration:none'
                                                                                                                            href='{0}'>Activar la cuenta ahora mismo</a></span>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </tbody>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td height='10'></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                                <td width='20'></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width='100%'
                                                                class='m_3095773769270076491blk_container m_3095773769270076491bmeHolder'
                                                                name='bmePreFooter' valign='top' align='center'
                                                                style='color:rgb(226,226,226;border:0px none transparent;background-color:rgb(255,255,255)'
                                                                bgcolor='#ffffff'>

                                                                <div id='m_3095773769270076491dv_16'>
                                                                    <table width='600' cellspacing='0' cellpadding='0'
                                                                        border='0' class='m_3095773769270076491blk'
                                                                        name='blk_divider'>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td
                                                                                    style='padding-top:20px;padding-bottom:20px;padding-left:20px;padding-right:20px'>
                                                                                    <table
                                                                                        class='m_3095773769270076491tblLine'
                                                                                        cellspacing='0' cellpadding='0'
                                                                                        border='0' width='100%'
                                                                                        style='border-top-width:0px;border-top-style:none;min-width:1px'>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td><span></span></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                                <div id='m_3095773769270076491dv_17'>
                                                                    <table width='600' cellspacing='0' cellpadding='0'
                                                                        border='0' class='m_3095773769270076491blk'
                                                                        name='blk_divider'>
                                                                        <tbody>
                                                                            <tr>
                                                                                <td
                                                                                    style='padding-top:20px;padding-bottom:20px;padding-left:20px;padding-right:20px'>
                                                                                    <table
                                                                                        class='m_3095773769270076491tblLine'
                                                                                        cellspacing='0' cellpadding='0'
                                                                                        border='0' width='100%'
                                                                                        style='border-top-width:0px;border-top-style:none;min-width:1px'>
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td><span></span></td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>", message.Content) };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                await client.SendAsync(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception, or both.
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
