using BuildCompleteEcommerceWithASPNETCoreMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn1.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) 
            : base(options)
        {
        }
        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<RoleAccount> RoleAccounts { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<SlideShow> SlideShows { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Photo> Photos { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<InvoiceDetails> InvoiceDetailses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.1.11-servicing-10062");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParents)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<RoleAccount>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.AccountId });

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.RoleAccounts)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role_Account"); 

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleAccounts)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Account_Role_Role");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Category_Product");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Photo");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Invoice_Account");
            });

            modelBuilder.Entity<InvoiceDetails>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceId, e.ProductId});
                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceDetailses)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InvoiceDetails_Invoice");

                entity.HasOne(d => d.Product)
                   .WithMany(p => p.InvoiceDetailses)
                   .HasForeignKey(d => d.ProductId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_InvoiceDetails_Product");
            });

        }
    }
}
