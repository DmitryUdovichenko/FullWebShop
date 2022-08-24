﻿using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<Order> Orders  { get; set; }
        public DbSet<OrderItem> OrderItems  { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods  { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.Entity.LastMidofiedDate = DateTime.Now;
                        entry.Entity.LastMidofiedBy = "test";
                        break;
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "test";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
