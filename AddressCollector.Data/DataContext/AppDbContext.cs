using System;
using System.Linq;
using System.Threading.Tasks;
using AddressCollector.Data.Auth;
using AddressCollector.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AddressCollector.Data.DataContext
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        
        public DbSet<Address> Address { get; set; }
        public DbSet<PostalCode> PostalCode { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Envelope> Envelope { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //fill db with data
        }

        public override int SaveChanges()
        {
            return InnerSaveChanges();
        }

        public int InnerSaveChanges(bool autoDetectChanges = true)
        {
            ChangeTracker.AutoDetectChangesEnabled = autoDetectChanges;
            WriteChangeSet();
            return base.SaveChanges();
        }

        private void WriteChangeSet()
        {
            WriteCreated();
            WriteLastUpdate();
        }

        private void WriteLastUpdate()
        {
            Parallel.ForEach(ChangeTracker.Entries()
                .Where(
                    c =>
                        c.Entity is IAuditableEntity && c.State == EntityState.Modified && c.State != EntityState.Added &&
                        c.State != EntityState.Deleted)
                .Select(c => c.Entity as IAuditableEntity), auditableEntity =>
            {
                auditableEntity.LastUpdateDate = DateTime.Now;
            });
        }

        private void WriteCreated()
        {
            Parallel.ForEach(ChangeTracker.Entries()
                .Where(c => c.Entity is IAuditableEntity && c.State == EntityState.Added)
                .Select(c => c.Entity as IAuditableEntity), auditableEntity =>
            {
                auditableEntity.CreateDate = DateTime.Now;
                auditableEntity.LastUpdateDate = DateTime.Now;
            });
        }
    }
}
