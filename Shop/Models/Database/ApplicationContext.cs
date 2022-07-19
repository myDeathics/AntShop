using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models.Database
{
    public class ApplicationContext : DbContext
    {   
        public DbSet<user> users { get; set; } = null!;
        public DbSet<basket> baskets { get; set; } = null!;
        public DbSet<basket_product> basket_products { get; set; } = null!;
        public DbSet<feedback> feedbacks { get; set; } = null!;
        public DbSet<product> products { get; set; } = null!;
        public DbSet<product_info> product_infos { get; set; } = null!;
        public DbSet<subtype> subtypes { get; set; } = null!;
        public DbSet<type> types { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<product>()
                .HasOne(u => u.type)
                .WithMany(c => c.products)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
