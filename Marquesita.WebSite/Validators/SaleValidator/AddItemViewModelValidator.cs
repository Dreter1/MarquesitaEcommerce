using FluentValidation;
using Marquesita.Infrastructure.ViewModels.Dashboards.Sales;

namespace Marquesita.WebSite.Validators.SaleValidator
{
    public class AddItemViewModelValidator : AbstractValidator<AddItemViewModel>
    {
        public AddItemViewModelValidator()
        {
            RuleFor(x => x.Quantity).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Quantity).Must(IsGreateThan0).WithMessage("La cantidad no puede ser ni 0 ni negativo");
            }).WithMessage("Cantidad no puede ser vacia");
        }

        private bool IsGreateThan0(int quantity)
        {
            if (quantity <= 0)
                return false;
            else
                return true;
        }
    }
}
