using FluentValidation;
using Marquesita.Infrastructure.ViewModels.Dashboards;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarquesitaDashboards.Validators
{
    public class RoleEditViewModelValidator : AbstractValidator<RoleEditViewModel>
    {
        public RoleEditViewModelValidator(RoleManager<Role> roleManager)
        {
            RuleFor(x => x.Name).NotEmpty().DependentRules(() => {
                RuleFor(x => x.Name).Must(name => {
                    var role = roleManager.Roles.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
                    return role == null;
                }).WithMessage("Este Rol ya existe, escoja otro");
            }).WithMessage("El campo del nombre no puede estar vacio");
        }
    }
}
