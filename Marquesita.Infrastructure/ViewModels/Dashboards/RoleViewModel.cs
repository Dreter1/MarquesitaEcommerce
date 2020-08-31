using Marquesita.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Dashboards
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
