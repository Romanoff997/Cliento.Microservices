using Cliento.Microservices.CRMService.Models;
using Microsoft.EntityFrameworkCore;

namespace Cliento.Microservices.CRMService.Data
{

    public class UserCrmDbContext : DbContext
    {
        public UserCrmDbContext(DbContextOptions<UserCrmDbContext> opts) : base(opts) { }
        public DbSet<UserCrm> Users => Set<UserCrm>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCrm>().HasKey(c => c.Id);
        }
    }
}
