using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Dashboards
{
    public class ResetEmployeePassword
    {
        [Required(ErrorMessage = "Contraseña requerida")]
        [DisplayName("Nueva Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DisplayName("Confirme nueva Contraseña")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }
}
