using Microsoft.AspNetCore.Identity;

namespace UserAdmin.Data.Entities
{
    public class User : IdentityUser
    {
        // Add additional properties here if needed
        [PersonalData]
        public string? FirstName { get; set; }
        [PersonalData]
        public string? LastName { get; set; }
        [PersonalData]
        public DateTime DateOfBirth { get; set; }
        [PersonalData]
        public string? Address { get; set; }
        [PersonalData]
        public string? City { get; set; }
        [PersonalData]
        public string? State { get; set; }
        [PersonalData]
        public string? Country { get; set; }
        [PersonalData]
        public string? PostalCode { get; set; }
    }
}