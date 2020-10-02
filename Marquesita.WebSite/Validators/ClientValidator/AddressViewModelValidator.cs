using FluentValidation;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marquesita.WebSite.Validators.ClientValidator
{
    public class AddressViewModelValidator : AbstractValidator<AddressViewModel>
    {
        public AddressViewModelValidator()
        {
            RuleFor(x => x.FullNames).NotEmpty().DependentRules(() => {
                RuleFor(x => x.FullNames).Matches(@"^[a-zA-Z\s]*$").WithMessage("Solo se puede ingresar letras");
            }).WithMessage("Nombre y apellidos no puede estar vacio escriba uno");

            RuleFor(x => x.Phone).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Phone).MinimumLength(9).WithMessage("Minimo 9 digitos");
                RuleFor(x => x.Phone).MaximumLength(12).WithMessage("Maximo 12 digitos");
                RuleFor(x => x.Phone).Matches(@"^(\(?\+?[0-9]*\)?)?[0-9_\ \(\)]*$").WithMessage("Solo se puede ingresar numeros ejemplo +51 123456789");
            }).WithMessage("El campo celular no puede estar vacio");

            RuleFor(x => x.Street).NotEmpty().WithMessage("Calle no puede estar vacio escriba uno");
            RuleFor(x => x.Region).NotEmpty().WithMessage("La region no puede estar vacio escriba uno");
            RuleFor(x => x.City).NotEmpty().WithMessage("La ciudad no puede estar vacio escriba uno");
            RuleFor(x => x.PostalCode).NotEmpty().WithMessage("Código Postal no puede estar vacio escriba uno");
        }
    }
}
