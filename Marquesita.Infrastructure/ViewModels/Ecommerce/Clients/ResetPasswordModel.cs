using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Clients
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Contraseña requerida")]
        [DisplayName("Nueva Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        [DisplayName("Confirme nueva Contraseña")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
