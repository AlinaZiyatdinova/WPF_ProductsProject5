using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WPF5.Models;

public partial class Fabric2Context : DbContext
{
    public Fabric2Context()
    {
    }

    public Fabric2Context(DbContextOptions<Fabric2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<PickupPoint> PickupPoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductManufacture> ProductManufactures { get; set; }

    public virtual DbSet<ProductProvider> ProductProviders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserOrder> UserOrders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-D0U93NL\\SQLEXPRESS; Database=Fabric2; Integrated Security = true; TrustServerCertificate = true ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");

            entity.Property(e => e.OrderId).ValueGeneratedNever();
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderDeliveryDate).HasColumnType("datetime");

            entity.HasOne(d => d.OrderPickupPoint).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderPickupPointId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_PickupPoint");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_OrderStatus");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_User");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductArticleNumber }).HasName("PK__OrderPro__817A2662F17505A8");

            entity.ToTable("OrderProduct");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductArticleNumber).HasMaxLength(100);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrderProduct_Order");

            entity.HasOne(d => d.ProductArticleNumberNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderProd__Produ__2E1BDC42");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.ToTable("OrderStatus");

            entity.Property(e => e.OrderStatusId).ValueGeneratedNever();
        });

        modelBuilder.Entity<PickupPoint>(entity =>
        {
            entity.HasKey(e => e.PickUpId);

            entity.ToTable("PickupPoint");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductArticleNumber).HasName("PK__Product__2EA7DCD52E51452C");

            entity.ToTable("Product");

            entity.Property(e => e.ProductArticleNumber).HasMaxLength(100);

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategories");

            entity.HasOne(d => d.ProductManufacturer).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductManufacturerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductManufactures");

            entity.HasOne(d => d.ProductProvider).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductProviders");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategriesId);

            entity.Property(e => e.CategriesId).ValueGeneratedNever();
        });

        modelBuilder.Entity<ProductManufacture>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId);

            entity.Property(e => e.ManufacturerId).ValueGeneratedNever();
        });

        modelBuilder.Entity<ProductProvider>(entity =>
        {
            entity.HasKey(e => e.ProviderId);

            entity.Property(e => e.ProviderId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__8AFACE3AC6505B86");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC28BAC790");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserPatronymic).HasMaxLength(100);
            entity.Property(e => e.UserSurname).HasMaxLength(100);

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__User__UserRole__52593CB8");
        });

        modelBuilder.Entity<UserOrder>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.UserId });

            entity.HasOne(d => d.User).WithMany(p => p.UserOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserOrders_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
