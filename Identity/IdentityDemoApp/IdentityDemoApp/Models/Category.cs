using System;
using System.ComponentModel.DataAnnotations;

namespace IdentityDemoApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public int Order { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
