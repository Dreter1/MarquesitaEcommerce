using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Dashboards
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La Contraseña es obligatorio")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
