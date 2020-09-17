using FluentValidation;
using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.ViewModels.Dashboards.Category;
using System.Linq;

namespace MarquesitaDashboards.Validators.CategoryValidator
{
    public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
    {
        public CategoryViewModelValidator(BusinessDbContext context)
        {
            RuleFor(x => x.Name).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Name).Must(name => {
                    var category = context.Categories.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
                    return category == null;
                }).WithMessage("Esta Categoria ya existe, escoja otro nombre");
                RuleFor(x => x.Name).Matches(@"^[a-zA-Z\s]*$").WithMessage("Solo se puede ingresar letras");
            }).WithMessage("El campo del nombre no puede estar vacio");
        }
    }
}
