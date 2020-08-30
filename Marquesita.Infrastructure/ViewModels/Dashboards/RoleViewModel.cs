using Marquesita.Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Dashboards
{
    public class RoleViewModel
    {
        public string Id { get; }
        public string Name { get; set; }
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
