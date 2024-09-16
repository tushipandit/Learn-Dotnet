using Microsoft.Build.Framework;

namespace IdentityDemoApp.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }


    }
}
