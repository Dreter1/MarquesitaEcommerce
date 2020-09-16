using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Clients
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("Correo")]
        public string Email { get; set; }
    }
}
