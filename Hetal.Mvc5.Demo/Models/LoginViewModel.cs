using System.ComponentModel.DataAnnotations;

namespace Hetal.Mvc5.Demo.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}