using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.Users
{
  public class UpdateModel
    {
        [Required]
        public string Email { get; set; }
        [Required, MinLength(10)]
        public string Password { get; set; }
    }
}