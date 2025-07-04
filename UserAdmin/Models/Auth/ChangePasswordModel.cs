using System.ComponentModel.DataAnnotations;

namespace UserAdmin.Models.Auth;

public class ChangePasswordModel
{
    [Required(ErrorMessage = "Current password is required")]
    [MinLength(8, ErrorMessage = "Current password must be at least 8 characters long")]
    public string? CurrentPassword { get; set; }
    [Required(ErrorMessage = "New password is required")]
    [MinLength(8, ErrorMessage = "New password must be at least 8 characters long")]
    public string? NewPassword { get; set; }
    [Required(ErrorMessage = "Confirm new password is required")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    [MinLength(8, ErrorMessage = "Confirm new password must be at least 8 characters long")]
    public string? ConfirmNewPassword { get; set; }
    [Required(ErrorMessage = "User name is required")]
    public string? UserName { get; set; }
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? Email { get; set; }
}