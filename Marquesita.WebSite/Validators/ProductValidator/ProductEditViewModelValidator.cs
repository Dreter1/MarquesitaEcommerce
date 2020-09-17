using FluentValidation;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;

namespace Marquesita.WebSite.Validators.ProductValidator
{
    public class ProductEditViewModelValidator : AbstractValidator<ProductEditViewModel>
    {
        public ProductEditViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Name).Matches(@"^[a-zA-Z\s]*$").WithMessage("Solo se puede ingresar letras");
            }).WithMessage("El campo del nombre no puede estar vacio");

            RuleFor(x => x.Description).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Description).MinimumLength(5).WithMessage("Minimo 5 caracteres");
                RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Maximo 1000 caracteres");
            }).WithMessage("La descripcion no puede estar vacio.");

            RuleFor(x => x.UnitPrice).NotEmpty().DependentRules(() => {
                RuleFor(x => x.UnitPrice.ToString()).Matches(@"^[0-9]*\.?[0-9]+$").WithMessage("Solo puede ingresar numeros enteros o con decimal");
            }).WithMessage("El precio no puede ir vacio.");

            RuleFor(x => x.Stock).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Stock.ToString()).Matches(@"[0-9]*$").WithMessage("Solo puede ingresar numeros enteros");
            }).WithMessage("El stock no puede ir vacio.");
        }
    }
}
