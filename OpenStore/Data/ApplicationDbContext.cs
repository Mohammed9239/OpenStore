using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OpenStore.Models;

namespace OpenStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ADS> ADS { get; set; }
        public DbSet<CategoryLev1> CategoryLev1s { get; set; }
        public DbSet<CategoryLev2> CategoryLev2s { get; set; }
        public DbSet<CategoryLev3> CategoryLev3s { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Img> Imgs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<ProdectProperty> ProdectProperties { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PropertyLev1> PropertyLev1s { get; set; }
        public DbSet<PropertyLev2> PropertyLev2s { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ProdectProperty>()
                .HasOne(i => i.Product).WithMany(i => i.ProdectProperties)
                .HasForeignKey(i=>i.ProductID).OnDelete(DeleteBehavior.ClientCascade);

            builder.Entity<ProdectProperty>()
                .HasOne(i => i.PropertyLev2).WithMany(i => i.ProdectProperties)
                .HasForeignKey(i=>i.PropertyLev2ID).OnDelete(DeleteBehavior.ClientCascade);

        }

    }
}
