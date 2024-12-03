using System.ComponentModel.DataAnnotations;
using RecipeMeal.Core.Enums;

namespace RecipeMeal.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public Role Role { get; set; } // Enum-based role

        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
    }
}
