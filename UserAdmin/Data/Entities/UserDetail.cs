using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace UserAdmin.Data.Entities
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(150)")]
        public string? FirstName { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(150)")]
        public string? LastName { get; set; }
        [PersonalData]
        public DateTime DateOfBirth { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(500)")]
        public string? Address { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(150)")]
        public string? City { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(2)")]
        public string? State { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(150)")]
        public string? Country { get; set; }
        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string? PostalCode { get; set; }
    }
}