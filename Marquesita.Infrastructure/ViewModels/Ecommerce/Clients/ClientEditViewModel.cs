using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Marquesita.Models.Identity;
using Microsoft.AspNetCore.Http;

namespace Marquesita.Infrastructure.ViewModels.Ecommerce.Clients
{
     public class ClientEditViewModel
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

        [DisplayName("Foto")]
        public string ImageRoute { get; set; }

        [DisplayName("Upload")]
        public IFormFile ProfileImage { get; set; }

        public DateTime DateOfBirth { get; set; }

        public static implicit operator User(ClientEditViewModel obj)
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
