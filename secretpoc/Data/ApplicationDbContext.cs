using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using secretpoc.Models;

namespace secretpoc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PortalUserBalance> PortalUserBalance { get; set; }
        public DbSet<PortalUser> PortalUser { get; set; }
    }
}
