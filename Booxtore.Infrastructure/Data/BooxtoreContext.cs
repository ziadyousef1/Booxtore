using System;
using System.Collections.Generic;
using Booxtore.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Booxtore.Infrastructure.Data;

public partial class BooxtoreContext : IdentityDbContext<ApplicationUser>
{
    public BooxtoreContext()
    {
    }

    public BooxtoreContext(DbContextOptions<BooxtoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowingRecord> BorrowingRecords { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<ReadingSession> ReadingSessions { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<UserLibrary> UserLibraries { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Biography).HasMaxLength(1000);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(300);
            entity.Property(e => e.Isbn).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.CoverImageUrl).HasMaxLength(500);
            entity.Property(e => e.PdfFileUrl).HasMaxLength(500);
            
            entity.HasOne(d => d.Author)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId);
                
            entity.HasOne(d => d.Category)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<BorrowingRecord>(entity =>
        {
            entity.HasKey(e => e.BorrowId);
            entity.Property(e => e.Status).HasMaxLength(50);
            
            entity.HasOne(d => d.Book)
                .WithMany(p => p.BorrowingRecords)
                .HasForeignKey(d => d.BookId);
                
            entity.HasOne(d => d.User)
                .WithMany(p => p.BorrowingRecords)
                .HasForeignKey(d => d.UserId);
        });

        // Configure Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // Configure OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.ItemId);
            
            entity.HasOne(d => d.Book)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.BookId);
                
            entity.HasOne(d => d.Order)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId);
        });

        // Configure PurchaseOrder
        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);
            entity.Property(e => e.PaymentStatus).HasMaxLength(50);
            entity.Property(e => e.PaymentMethod).HasMaxLength(100);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            
            entity.HasOne(d => d.User)
                .WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.UserId);
        });

        // Configure ReadingSession
        modelBuilder.Entity<ReadingSession>(entity =>
        {
            entity.HasKey(e => e.SessionId);
            
            entity.HasOne(d => d.Book)
                .WithMany(p => p.ReadingSessions)
                .HasForeignKey(d => d.BookId);
                
            entity.HasOne(d => d.User)
                .WithMany(p => p.ReadingSessions)
                .HasForeignKey(d => d.UserId);
        });

        // Configure Review
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId);
            entity.Property(e => e.ReviewText).HasMaxLength(2000);
            entity.Property(e => e.Rating).HasColumnType("decimal(3,2)");
            
            entity.HasOne(d => d.Book)
                .WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId);
                
            entity.HasOne(d => d.User)
                .WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId);
        });

        // Configure UserLibrary
        modelBuilder.Entity<UserLibrary>(entity =>
        {
            entity.HasKey(e => e.LibraryId);
            entity.Property(e => e.AccessType).HasMaxLength(50);
            
            entity.HasOne(d => d.Book)
                .WithMany(p => p.UserLibraries)
                .HasForeignKey(d => d.BookId);
                
            entity.HasOne(d => d.User)
                .WithMany(p => p.UserLibraries)
                .HasForeignKey(d => d.UserId);
        });
    }

}
