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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<User>(entity =>
            {
              
                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.UserName, "UQ__Users__UserName")
                      .IsUnique(); 

                entity.Property(e => e.UserName)
                      .IsRequired()
                      .HasMaxLength(50)
                      .IsUnicode(false);

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

               
                entity.HasMany(u => u.Expenses)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserName) 
                      .HasPrincipalKey(u => u.UserName) 
                      .OnDelete(DeleteBehavior.Cascade); 
            });


            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => e.ExpenseId);

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

                entity.Property(e => e.UserName)
                      .IsRequired()
                      .HasMaxLength(50)
                      .IsUnicode(false);

               
                entity.HasOne(e => e.User)
                      .WithMany(u => u.Expenses)
                      .HasForeignKey(e => e.UserName) 
                      .HasPrincipalKey(u => u.UserName) 
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
