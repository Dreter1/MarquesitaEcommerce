using Marquesita.Infrastructure.EmailConfigurations.Models;
using Marquesita.Infrastructure.EmailConfigurations.Interfaces;

namespace Marquesita.Infrastructure.EmailConfigurations.Services
{
    public class EmailsTextService : IEmailsTextService
    {
        public string ConfirmMailTextEcommerce(Message message)
        {
            string emailText = string.Format(@"<table  border='0' cellpadding='0' cellspacing='0' width='100%'>
        <tr>
            <td bgcolor='#FFA73B' align='center'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                    <tr>
                        <td align='center' valign='top' style='padding: 40px 10px 40px 10px;'> </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td bgcolor='#FFA73B' align='center' style='padding: 0px 10px 0px 10px;'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                    <tr>
                        <td bgcolor='#ffffff' align='center' valign='top' style='padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;'>
                            <h1 style='font-size: 48px; font-weight: 400; margin: 2;'>Bienvenido!</h1>
                            <img src='https://img.icons8.com/clouds/100/000000/handshake.png' width='125' height='120' style='display: block; border: 0px;' />
                             <h2 style='font-size: 35px; font-weight: 400; color: #111111; margin: 0;'>Active su cuenta</h2>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       <tr>
            <td bgcolor='#f4f4f4' align='center' style='padding: 0px 10px 0px 10px;'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                    <tr>
                        <td bgcolor='#ffffff' align='left' style='padding: 20px 30px 40px 30px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                            <p style='margin: 0;'>Estimado/a {1} {2}, le damos la bienvenida a La Marquesita. Primero, debes confirmar tu cuenta, para que pueda hacer compras y demás. Simplemente presione el botón de abajo.</p>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor='#ffffff' align='left'>
                            <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td bgcolor='#ffffff' align='center' style='padding: 20px 30px 60px 30px;'>
                                        <table border='0' cellspacing='0' cellpadding='0'>
                                            <tr>
                                                <td align='center' style='border-radius: 3px;' bgcolor='#FFA73B'><a href='{0}' target='_blank' style='font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;'>Activar la cuenta ahora mismo</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr> <!-- COPY -->
                    <tr>
                        <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 20px 30px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                            <p style='margin: 0;'>Si tiene alguna pregunta, envíe un correo electrónico haciendo <a href='mailto: contacto.lamarquesita@gmail.com'>clic aquí</a> ó escribanos directo a: contacto.lamarquesita@gmail.com, siempre estaremos encantados de ayudarle.</p>
                            <p style='margin: 0;'>* Por favor no responder a este correo, gracias</p>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                            <p style='margin: 0;'>Saludos,<br>La Marquesita</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
</table>", message.Content, message.User.FirstName, message.User.LastName);
            return emailText;
        }

        public string ConfirmMailTextShop(Message message)
        {
            string emailText = string.Format(@"<table  border='0' cellpadding='0' cellspacing='0' width='100%'>
        <tr>
            <td bgcolor='#FFA73B' align='center'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                    <tr>
                        <td align='center' valign='top' style='padding: 40px 10px 40px 10px;'> </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td bgcolor='#FFA73B' align='center' style='padding: 0px 10px 0px 10px;'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                    <tr>
                        <td bgcolor='#ffffff' align='center' valign='top' style='padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;'>
                            <h1 style='font-size: 48px; font-weight: 400; margin: 2;'>Bienvenido!</h1>
                            <img src='https://img.icons8.com/clouds/100/000000/handshake.png' width='125' height='120' style='display: block; border: 0px;' />
                             <h2 style='font-size: 35px; font-weight: 400; color: #111111; margin: 0;'>Active su cuenta</h2>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       <tr>
            <td bgcolor='#f4f4f4' align='center' style='padding: 0px 10px 0px 10px;'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                    <tr>
                        <td bgcolor='#ffffff' align='left' style='padding: 20px 30px 40px 30px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                            <p style='margin: 0;'>Estimado/a {1} {2}, le damos la bienvenida a La Marquesita. Primero, debes confirmar tu cuenta, para que pueda hacer compras y demás. Simplemente presione el botón de abajo.</p>
                            <p style='margin: 0;'>Su usuario es: {3}, para que pueda ingresar a la tienda virtual, se ha creado su cuenta por tienda, para que pueda cambiar su contraseña haga <a href='{4}'><strong>Click aqui</strong><a/></p>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor='#ffffff' align='left'>
                            <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td bgcolor='#ffffff' align='center' style='padding: 20px 30px 60px 30px;'>
                                        <table border='0' cellspacing='0' cellpadding='0'>
                                            <tr>
                                                <td align='center' style='border-radius: 3px;' bgcolor='#FFA73B'><a href='{0}' target='_blank' style='font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;'>Activar la cuenta ahora mismo</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr> <!-- COPY -->
                    <tr>
                        <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 20px 30px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                            <p style='margin: 0;'>Si tiene alguna pregunta, envíe un correo electrónico haciendo <a href='mailto: contacto.lamarquesita@gmail.com'>clic aquí</a> ó escribanos directo a: contacto.lamarquesita@gmail.com, siempre estaremos encantados de ayudarle.</p>
                            <p style='margin: 0;'>* Por favor no responder a este correo, gracias</p>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                            <p style='margin: 0;'>Saludos,<br>La Marquesita</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
</table>", message.Content, message.User.FirstName, message.User.LastName, message.User.UserName, message.OptionalURL);
            return emailText;
        }

        public string RecoveryPasswordText(Message message)
        {
            string emailPasswordText = string.Format(@"<table  border='0' cellpadding='0' cellspacing='0' width='100%'>
        <tr>
            <td bgcolor='#FFA73B' align='center'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                    <tr>
                        <td align='center' valign='top' style='padding: 40px 10px 40px 10px;'> </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td bgcolor='#FFA73B' align='center' style='padding: 0px 10px 0px 10px;'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                    <tr>
                        <td bgcolor='#ffffff' align='center' valign='top' style='padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;'>
                            <h1 style='font-size: 48px; font-weight: 400; margin: 2;'>Hola {1} {2}</h1>
                            <img src=' https://img.icons8.com/clouds/100/000000/handshake.png' width='125' height='120' style='display: block; border: 0px;' />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       <tr>
            <td bgcolor='#f4f4f4' align='center' style='padding: 0px 10px 0px 10px;'>
                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
                    <tr>
                        <td bgcolor='#ffffff' align='left' style='padding: 20px 30px 40px 30px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                            <p style='margin: 0;'>Haz clic en el siguiente botón para restablecer tu contraseña. Si no has solicitado una nueva contraseña, ignora este correo.</p>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor='#ffffff' align='left'>
                            <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td bgcolor='#ffffff' align='center' style='padding: 20px 30px 60px 30px;'>
                                        <table border='0' cellspacing='0' cellpadding='0'>
                                            <tr>
                                                <td align='center' style='border-radius: 3px;' bgcolor='#FFA73B'><a href='{0}' target='_blank' style='font-size: 20px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; color: #ffffff; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #FFA73B; display: inline-block;'>Restablecer contraseña</a></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr> <!-- COPY -->
                    <tr>
                        <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 20px 30px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                            <p style='margin: 0;'>Si tiene alguna pregunta, envíe un correo electrónico haciendo <a href='mailto: contacto.lamarquesita@gmail.com'>clic aquí</a> ó escribanos directo a: contacto.lamarquesita@gmail.com, siempre estaremos encantados de ayudarle.</p>
                            <p style='margin: 0;'>* Por favor no responder a este correo, gracias</p>
                        </td>
                    </tr>
                    <tr>
                        <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'>
                            <p style='margin: 0;'>Saludos,<br>La Marquesita</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
</table>", message.Content, message.User.FirstName, message.User.LastName);
            return emailPasswordText;
        }

        public string SaleConfirmationText(Message message)
        {
            string saleConfirmationText = string.Format(@"<table border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td bgcolor='#FFA73B' align='center'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td align='center' valign='top' style='padding: 40px 10px 40px 10px;'> </td></tr></table> </td></tr><tr> <td bgcolor='#FFA73B' align='center' style='padding: 0px 10px 0px 10px;'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td bgcolor='#ffffff' align='center' valign='top' style='padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;'> <h1 style='font-size: 48px; font-weight: 400; margin: 2;'>Hola{1} {2}</h1> <img src='https://img.icons8.com/clouds/2x/shopping-cart.png' width='125' height='120' style='display: block; border: 0px;'/> </td></tr></table> </td></tr><tr> <td bgcolor='#f4f4f4' align='center' style='padding: 0px 10px 0px 10px;'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td bgcolor='#ffffff' align='left' style='padding: 20px 30px 40px 30px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'> <p style='margin: 0;'>Estamos muy felices de poder haberte atendido y que prefieras nuestros productos.</p><br <p style='margin: 0;'>Gracias por la confianza, te enviamos el detalle de tu compra para que puedas descargar tu boleta o imprimirla.</p></td></tr><tr> <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 20px 30px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'> <p style='margin: 0;'>Si tiene alguna pregunta, envíe un correo electrónico haciendo <a href='mailto: contacto.lamarquesita@gmail.com'>clic aquí</a> ó escribanos directo a: contacto.lamarquesita@gmail.com, siempre estaremos encantados de ayudarle.</p><p style='margin: 0;'>*<strong><u>Por favor no responder a este correo, gracias</u></strong></p></td></tr><tr> <td bgcolor='#ffffff' align='left' style='padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: Lato, Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'> <p style='margin: 0;'>Saludos</p><hr> <div style='text-align: center!important;'> <img src='https://i.ibb.co/8gcZ0wS/Marquesita-Logo.jpg' alt='Marquesita-Logo' style='width: 200px; height: 100px; border: 0;' > </div></td></tr></table> </td></tr></table>"
            , message.Content, message.User.FirstName, message.User.LastName);
            return saleConfirmationText;
        }
    }
}
