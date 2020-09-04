using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Marquesita.Models.Business;

namespace Marquesita.Infrastructure.ViewModels.Dashboards.Category
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nombre Requerido")]
        [DisplayName("Nombre")]
        public string Name { get; set; }

        public static implicit operator Categories(CategoryViewModel obj) {
            return new Categories
            {
                Id = obj.Id,
                Name = obj.Name
            };
        }
    }
}
