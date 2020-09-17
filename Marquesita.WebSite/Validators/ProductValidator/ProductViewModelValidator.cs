using FluentValidation;
using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.ViewModels.Ecommerce.Products;
using System.Linq;

namespace Marquesita.WebSite.Validators.ProductValidator
{
    public class ProductViewModelValidator : AbstractValidator<ProductViewModel>
    {
        public ProductViewModelValidator(BusinessDbContext context)
        {
            RuleFor(x => x.Name).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Name).Must(name => {
                    var product = context.Products.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
                    return product == null;
                }).WithMessage("Este producto ya existe, escoja otro nombre");
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

            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Porfavor escoja una categoria.");

        }
    }
}
