using System;
using Microsoft.EntityFrameworkCore;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data
{
    public partial class AsiBasecodeDBContext : DbContext
    {
        public AsiBasecodeDBContext(DbContextOptions<AsiBasecodeDBContext> options)
            : base(options)
        {
        }
            
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Product> Products { get; set; }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Entity Configuration
            modelBuilder.Entity<User>(entity =>
            {
                // Configure primary key
                entity.HasKey(e => e.Id);

                // Ensure UserName is unique
                entity.HasIndex(e => e.UserName, "UQ__Users__UserName")
                      .IsUnique();

                // Configure fields
                entity.Property(e => e.UserName)
                      .IsRequired()
                      .HasMaxLength(50)
                      .IsUnicode(false); // Ensure this property is the same type as in Expense

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.Password)
                      .IsRequired()
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.CreatedTime)
                      .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy)
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.UpdatedTime)
                      .HasColumnType("datetime");

                // Configure relationship between User and Expenses
                entity.HasMany(u => u.Expenses)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserName) // Foreign key in Expense referencing UserName in User
                      .HasPrincipalKey(u => u.UserName) // Principal key in User entity
                      .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete
            });

            // Expense Entity Configuration
            modelBuilder.Entity<Expense>(entity =>
            {
                // Configure primary key
                entity.HasKey(e => e.ExpenseId);

                // Configure fields
                entity.Property(e => e.ExpenseId)
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(100)
                      .IsUnicode(false);

                entity.Property(e => e.Category)
                      .IsRequired()
                      .HasMaxLength(50)
                      .IsUnicode(false);

                entity.Property(e => e.Amount)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(e => e.Date)
                      .HasColumnType("datetime")
                      .IsRequired();

                entity.Property(e => e.UserName) // Ensure this is properly configured as a string
                      .IsRequired()
                      .HasMaxLength(50)
                      .IsUnicode(false);

                // Configure relationship between Expense and User
                entity.HasOne(e => e.User) // Each Expense references a single User
                      .WithMany(u => u.Expenses) // A User can have many Expenses
                      .HasForeignKey(e => e.UserName) // Foreign key in Expense referencing UserName in User
                      .HasPrincipalKey(u => u.UserName) // Principal key in User
                      .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete
            });

            // Call any additional configurations
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
