using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Terra.Models;

namespace Terra.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Terra.Models.Characters> Character { get; set; }
        public DbSet<Terra.Models.Monster> Monsters { get; set; }  
        public DbSet<Terra.Models.Game> Games { get; set; }  
        
        // protected override void OnModelCreating(ModelBuilder builder)
        // {
        //     base.OnModelCreating(builder);
        //     // Customize the ASP.NET Identity model and override the defaults if needed.
        //     // For example, you can rename the ASP.NET Identity table names and more.
        //     // Add your customizations after calling base.OnModelCreating(builder);
        // }
    }
}
