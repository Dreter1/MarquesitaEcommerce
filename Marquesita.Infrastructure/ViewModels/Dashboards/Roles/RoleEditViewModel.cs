using Marquesita.Models.Identity;
using System.ComponentModel;

namespace Marquesita.Infrastructure.ViewModels.Dashboards.Roles
{
    public class RoleEditViewModel
    {
        public string Id { get; }

        [DisplayName("Nombre")]
        public string Name { get; set; }

        public static implicit operator Role(RoleEditViewModel obj)
        {
            return new Role
            {
                Id = obj.Id,
                Name = obj.Name
            };
        }
    }
}
