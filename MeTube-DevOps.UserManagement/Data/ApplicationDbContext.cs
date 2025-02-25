using System;
using Microsoft.EntityFrameworkCore;
using MeTube_DevOps.UserManagement.Entities;

namespace MeTube_DevOps.UserManagement.Data;

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

    }
}
