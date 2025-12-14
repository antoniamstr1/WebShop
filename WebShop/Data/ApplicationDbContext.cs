using Microsoft.EntityFrameworkCore;
using WebShop.Models;
using WebShop.Entities;
namespace WebShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<ProductInCart> ProductInCarts => Set<ProductInCart>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Order> Orders { get; set; }

    }
}