using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Clients
{
    public class ClientViewModel
    {
        public string Id { get; set; }

        [DisplayName("Usuario")]
        public string Username { get; set; }

        [DisplayName("Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

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

        [DisplayName("Fecha de Nacimiento")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Foto")]
        public string ImageRoute { get; set; }

        public static implicit operator User(ClientViewModel obj)
        {
            return new User
            {
                Id = obj.Id,
                UserName = obj.Username,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                Phone = obj.Phone,
                ImageRoute = obj.ImageRoute,
                DateOfBirth = obj.DateOfBirth
            };
        }
    }
}
