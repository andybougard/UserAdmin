using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAdmin.Data.Entities;

namespace UserAdmin.Data;
public class DataContext : IdentityDbContext<IdentityUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    // Add DbSet properties for your entities here
    public DbSet<AppUser> AppUsers { get; set; }
}