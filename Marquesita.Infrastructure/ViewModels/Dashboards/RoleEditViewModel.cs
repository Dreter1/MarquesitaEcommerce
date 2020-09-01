using Marquesita.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Dashboards
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
