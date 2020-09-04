using FluentValidation;
using Marquesita.Infrastructure.DbContexts;
using Marquesita.Infrastructure.ViewModels.Dashboards.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Validators.CategoryValidators
{
    public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
    {
        public CategoryViewModelValidator(BusinessDbContext context)
        {
            RuleFor(x => x.Name).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Name).Must(name => {
                    var role = context.Categories.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
                    return role == null;
                }).WithMessage("Esta Categoria ya existe, escoja otro nombre");
            }).WithMessage("El campo del nombre no puede estar vacio");
        }
    }
}
