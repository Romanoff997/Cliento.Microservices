using Cliento.Microservices.CRMService.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Cliento.Microservices.CRMService.Data
{

    public class CrmDbContext : DbContext
    {
        public CrmDbContext(DbContextOptions<CrmDbContext> opts) : base(opts) { }
        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Session> Sessions => Set<Session>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Session>().HasKey(s => s.Id);
            modelBuilder.Entity<Client>().HasMany(c => c.Sessions).WithOne(s => s.Client).HasForeignKey(s => s.ClientId);
        }
    }
}
