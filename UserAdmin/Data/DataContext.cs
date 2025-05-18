using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAdmin.Data.Entities;

namespace UserAdmin.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // Add DbSet properties for your entities here
        // public DbSet<YourEntity> YourEntities { get; set; }
    }
}