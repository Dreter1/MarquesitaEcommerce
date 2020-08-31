using Marquesita.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Marquesita.Infrastructure.ViewModels.Dashboards
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [DisplayName("Usuario")]
        public string Username { get; set; }

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

        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Cargo")]
        public string Role { get; set; }

        [DisplayName("Fecha de Nacimiento")]
        public DateTime DateOfBirth { get; set; }
        public DateTime RegisterDate { get; set; }

        [DisplayName("Foto")]
        public string ImageRoute { get; set; }

        public static implicit operator User(UserViewModel obj)
        {
            return new User
            {
                Id = obj.Id,
                UserName = obj.Username,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Phone = obj.Phone,
                DateOfBirth = obj.DateOfBirth,
                RegisterDate = obj.RegisterDate,
                ImageRoute = obj.ImageRoute
            };
        }
    }
}
