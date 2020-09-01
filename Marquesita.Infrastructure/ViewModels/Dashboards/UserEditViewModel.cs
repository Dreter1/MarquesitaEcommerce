using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marquesita.Infrastructure.ViewModels.Dashboards
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [DisplayName("Nombres")]
        public string FirstName { get; set; }

        [DisplayName("Apellidos")]
        public string LastName { get; set; }

        [DisplayName("Correo")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Celular")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DisplayName("Cargo")]
        public string Role { get; set; }

        [DisplayName("Foto")]
        public string ImageRoute { get; set; }

        [DisplayName("Upload")]
        public IFormFile ProfileImage { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime RegisterDate { get; set; }

        public static implicit operator User(UserEditViewModel obj)
        {
            return new User
            {
                Id = obj.Id,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                ImageRoute = obj.ImageRoute,
                Phone = obj.Phone,
            };
        }
    }
}
