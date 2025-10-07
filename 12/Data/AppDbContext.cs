using Microsoft.EntityFrameworkCore;
using EFCoreApp.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EFCoreApp.Data
{
    public class AppDbContext : DbContext
    {
        // Конструктор для внедрения зависимостей
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet для каждой модели
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация модели Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");

                // Связь с Category
                entity.HasOne(p => p.Category)
                      .WithMany(c => c.Products)
                      .HasForeignKey(p => p.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Индекс для улучшения производительности
                entity.HasIndex(p => p.CategoryId);
                entity.HasIndex(p => p.IsActive);
            });

            // Конфигурация модели Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Description).HasMaxLength(200);

                // Уникальный индекс для имени категории
                entity.HasIndex(c => c.Name).IsUnique();
            });

            // Конфигурация модели Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(o => o.CustomerEmail).IsRequired();
                entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");

                // Индексы для улучшения производительности
                entity.HasIndex(o => o.CustomerEmail);
                entity.HasIndex(o => o.OrderDate);
                entity.HasIndex(o => o.Status);
            });

            // Конфигурация модели OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");

                // Связь с Order
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Связь с Product
                entity.HasOne(oi => oi.Product)
                      .WithMany()
                      .HasForeignKey(oi => oi.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Составной индекс
                entity.HasIndex(oi => new { oi.OrderId, oi.ProductId });
            });

            // Заполнение начальными данными
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Начальные категории
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Электроника", Description = "Электронные устройства и гаджеты" },
                new Category { Id = 2, Name = "Книги", Description = "Художественная и образовательная литература" },
                new Category { Id = 3, Name = "Одежда", Description = "Мужская и женская одежда" }
            };

            // Начальные продукты
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Смартфон Samsung", Description = "Флагманский смартфон", Price = 59999.99m, CategoryId = 1 },
                new Product { Id = 2, Name = "Ноутбук HP", Description = "Игровой ноутбук", Price = 89999.99m, CategoryId = 1 },
                new Product { Id = 3, Name = "Научная фантастика", Description = "Сборник научно-фантастических рассказов", Price = 899.99m, CategoryId = 2 },
                new Product { Id = 4, Name = "Джинсы", Description = "Мужские джинсы классического кроя", Price = 2999.99m, CategoryId = 3 },
                new Product { Id = 5, Name = "Футболка", Description = "Хлопковая футболка", Price = 999.99m, CategoryId = 3 }
            };

            modelBuilder.Entity<Category>().HasData(categories);
            modelBuilder.Entity<Product>().HasData(products);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Автоматическое обновление дат
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Product &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((Product)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
                }
                ((Product)entityEntry.Entity).UpdatedDate = DateTime.UtcNow;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}