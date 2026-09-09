using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    public class UserModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public string? CollegeName { get; set; }
    }
}