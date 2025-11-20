using System;
using System.Collections.Generic;
using Lab13AlberthMCuevas.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Lab13AlberthMCuevas.Infrastructure.Data.Context;

public partial class LinqContext : DbContext
{
    public LinqContext()
    {
    }

    public LinqContext(DbContextOptions<LinqContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    // Eliminar OnConfiguring - la configuración viene del Program.cs
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // PostgreSQL no necesita UseCollation ni HasCharSet
        // Solo configurar las entidades

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PRIMARY");

            entity.ToTable("clients");

            entity.Property(e => e.ClientId).HasColumnType("integer");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.ClientId, "ClientId");

            entity.Property(e => e.OrderId).HasColumnType("integer");
            entity.Property(e => e.ClientId).HasColumnType("integer");
            entity.Property(e => e.OrderDate).HasColumnType("timestamp");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_ibfk_1");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PRIMARY");

            entity.ToTable("orderdetails");

            entity.HasIndex(e => e.OrderId, "OrderId");

            entity.HasIndex(e => e.ProductId, "ProductId");

            entity.Property(e => e.OrderDetailId).HasColumnType("integer");
            entity.Property(e => e.OrderId).HasColumnType("integer");
            entity.Property(e => e.ProductId).HasColumnType("integer");
            entity.Property(e => e.Quantity).HasColumnType("integer");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderdetails_ibfk_1");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderdetails_ibfk_2");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("products");

            entity.Property(e => e.ProductId).HasColumnType("integer");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(10, 2);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}