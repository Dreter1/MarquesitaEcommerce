using Marquesita.Models.Identity;
using System.Collections.Generic;
using System.ComponentModel;

namespace Marquesita.Infrastructure.ViewModels.Dashboards.Roles
{
    public class RoleViewModel
    {
        public string Id { get; }

        [DisplayName("Nombre")]
        public string Name { get; set; }

        [DisplayName("Permisos")]
        public List<string> Permissions { get; set; }

        public static implicit operator Role(RoleViewModel obj)
        {
            return new Role
            {
                Name = obj.Name
            };
        }
    }
}
