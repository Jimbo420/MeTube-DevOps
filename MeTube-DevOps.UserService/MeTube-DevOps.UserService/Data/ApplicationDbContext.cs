using System;
using Microsoft.EntityFrameworkCore;
using MeTube_DevOps.UserService.Entities;

namespace MeTube_DevOps.UserService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(entity => entity.Username).IsRequired().HasMaxLength(20);
            entity.Property(entity => entity.Password).IsRequired().HasMaxLength(20);
            entity.Property(entity => entity.Email).IsRequired();
            entity.Property(entity => entity.Role).IsRequired().HasDefaultValue("User");
        });

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "Alice", Password = "password123", Email = "alice@example.com", Role = "Admin" },
            new User { Id = 2, Username = "Bob", Password = "securePass", Email = "bob@example.com", Role = "Admin" },
            new User { Id = 3, Username = "Charlie", Password = "pass456", Email = "charlie@example.com", Role = "User" },
            new User { Id = 4, Username = "Diana", Password = "dianaPass", Email = "diana@example.com", Role = "User" },
            new User { Id = 5, Username = "Eve", Password = "evePass123", Email = "eve@example.com", Role = "User" }
        );

    }
}
