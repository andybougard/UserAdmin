namespace UserAdmin.Models.User
{
    using System.ComponentModel.DataAnnotations;

    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long", MinimumLength = 6)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(150, ErrorMessage = "First name cannot be longer than 150 characters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(150, ErrorMessage = "Last name cannot be longer than 150 characters")]
        public string? LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot be longer than 500 characters")]
        public string? Address { get; set; }

        [StringLength(150, ErrorMessage = "City cannot be longer than 150 characters")]
        public string? City { get; set; }
    }
}