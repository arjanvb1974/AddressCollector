using AddressCollector.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AddressCollector.Models
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Pie> Pies { get; set; }
        //public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        //public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<PostalCode> PostalCode { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //fill db with data

            

            

            
            

            

            
            
        }
    }
}
