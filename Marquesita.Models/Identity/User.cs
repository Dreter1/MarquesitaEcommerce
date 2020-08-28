using Microsoft.AspNetCore.Identity;
using System;

namespace Marquesita.Models.Identity
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string MailToken { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; }
    }
}
