using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace IdentityDemoApp.Models
{
    public class RegisterViewModel
    {

        public int Id { get; set; } 
        public string UserName { get; set; }
        public string Password { get; set; }

        [EmailAddress]
        [Remote(action:"IsEmailInUse",controller:"Account")] //use for server side validation
        public string Email { get; set; }
        public string City { get; set; }

    }
}
