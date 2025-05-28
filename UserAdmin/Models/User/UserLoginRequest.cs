namespace UserAdmin.Models.User
{
    using System.ComponentModel.DataAnnotations;

    public class UserLoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long", MinimumLength = 6)]
        public string? Password { get; set; }
    }
}