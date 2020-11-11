using FluentValidation;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.WebSite.Validators.ClientValidator
{
    public class ClientEditViewModelValidator : AbstractValidator<ClientEditViewModel>
    {
        public ClientEditViewModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().DependentRules(() => {
                RuleFor(x => x.FirstName).Matches(@"^[a-zA-Z\s]*$").WithMessage("Solo se puede ingresar letras");
            }).WithMessage("Nombres no puede estar vacio escriba uno");

            RuleFor(x => x.LastName).NotEmpty().DependentRules(() => {
                RuleFor(x => x.FirstName).Matches(@"^[a-zA-Z\s]*$").WithMessage("Solo se puede ingresar letras");
            }).WithMessage("Apellidos no puede estar vacio escriba uno");

            RuleFor(x => x.Phone).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Phone).MinimumLength(9).WithMessage("Minimo 9 digitos");
                RuleFor(x => x.Phone).MaximumLength(12).WithMessage("Maximo 12 digitos");
                RuleFor(x => x.Phone).Matches(@"^(\(?\+?[0-9]*\)?)?[0-9_\ \(\)]*$").WithMessage("Solo se puede ingresar numeros ejemplo +51 123456789");
            }).WithMessage("El campo celular no puede estar vacio");

            RuleFor(x => x.Email).NotEmpty().DependentRules(() =>
            {
                RuleFor(x => x.Email).Matches(@"^(('[\w-\s]+')|([\w-]+(?:\.[\w-]+)*)|('[\w-\s]+')([\w-]+(?:\.[\w-]+)*))(@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$)|(@\[?((25[0-5]\.|2[0-4][0-9]\.|1[0-9]{2}\.|[0-9]{1,2}\.))((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\.){2}(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[0-9]{1,2})\]?$)").WithMessage("Ingrese un correo valido");
            }).WithMessage("El campo correo no puede estar vacio");
        }
    }
}
