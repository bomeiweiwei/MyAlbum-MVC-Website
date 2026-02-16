using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Infrastructure.EF.Models;

namespace MyAlbum.Infrastructure.EF.Data;

public partial class MyAlbumContext : DbContext
{
    public MyAlbumContext(DbContextOptions<MyAlbumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<AlbumCategory> AlbumCategories { get; set; }

    public virtual DbSet<AlbumComment> AlbumComments { get; set; }

    public virtual DbSet<AlbumPhoto> AlbumPhotos { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.HasIndex(e => new { e.AccountType, e.Status }, "IX_Account_AccountType_Status");

            entity.HasIndex(e => e.UserName, "UX_Account_UserName").IsUnique();

            entity.Property(e => e.AccountId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Account_CreatedAtUtc");
            entity.Property(e => e.LastLoginAtUtc).HasPrecision(3);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue((byte)1, "DF_Account_Status");
            entity.Property(e => e.UpdatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Account_UpdatedAtUtc");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<Album>(entity =>
        {
            entity.ToTable("Album");

            entity.HasIndex(e => new { e.AlbumCategoryId, e.Status, e.ReleaseTimeUtc }, "IX_Album_Category_Status_ReleaseTimeUtc").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.OwnerAccountId, e.Status, e.ReleaseTimeUtc }, "IX_Album_Owner_Status_ReleaseTimeUtc").IsDescending(false, false, true);

            entity.Property(e => e.AlbumId).ValueGeneratedNever();
            entity.Property(e => e.CoverPath).HasMaxLength(260);
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Album_CreatedAtUtc");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ReleaseTimeUtc).HasPrecision(3);
            entity.Property(e => e.Status).HasDefaultValue((byte)1, "DF_Album_Status");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.UpdatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Album_UpdatedAtUtc");

            entity.HasOne(d => d.AlbumCategory).WithMany(p => p.Albums)
                .HasForeignKey(d => d.AlbumCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Album_AlbumCategory");

            entity.HasOne(d => d.OwnerAccount).WithMany(p => p.Albums)
                .HasForeignKey(d => d.OwnerAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Album_Owner");
        });

        modelBuilder.Entity<AlbumCategory>(entity =>
        {
            entity.ToTable("AlbumCategory");

            entity.HasIndex(e => e.CategoryName, "UX_AlbumCategory_CategoryName").IsUnique();

            entity.Property(e => e.AlbumCategoryId).ValueGeneratedNever();
            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_AlbumCategory_CreatedAtUtc");
            entity.Property(e => e.Status).HasDefaultValue((byte)1, "DF_AlbumCategory_Status");
            entity.Property(e => e.UpdatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_AlbumCategory_UpdatedAtUtc");
        });

        modelBuilder.Entity<AlbumComment>(entity =>
        {
            entity.ToTable("AlbumComment");

            entity.HasIndex(e => new { e.AlbumPhotoId, e.Status, e.ReleaseTimeUtc }, "IX_AlbumComment_AlbumPhoto_Status_ReleaseTimeUtc").IsDescending(false, false, true);

            entity.HasIndex(e => new { e.MemberId, e.Status, e.ReleaseTimeUtc }, "IX_AlbumComment_Member_Status_ReleaseTimeUtc").IsDescending(false, false, true);

            entity.Property(e => e.AlbumCommentId).ValueGeneratedNever();
            entity.Property(e => e.Comment).HasMaxLength(2000);
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_AlbumComment_CreatedAtUtc");
            entity.Property(e => e.ReleaseTimeUtc).HasPrecision(3);
            entity.Property(e => e.Status).HasDefaultValue((byte)1, "DF_AlbumComment_Status");
            entity.Property(e => e.UpdatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_AlbumComment_UpdatedAtUtc");

            entity.HasOne(d => d.AlbumPhoto).WithMany(p => p.AlbumComments)
                .HasForeignKey(d => d.AlbumPhotoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlbumComment_AlbumPhoto");

            entity.HasOne(d => d.Member).WithMany(p => p.AlbumComments)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlbumComment_Member");
        });

        modelBuilder.Entity<AlbumPhoto>(entity =>
        {
            entity.ToTable("AlbumPhoto");

            entity.HasIndex(e => new { e.AlbumId, e.Status, e.SortOrder }, "IX_AlbumPhoto_Album_Status_SortOrder");

            entity.Property(e => e.AlbumPhotoId).ValueGeneratedNever();
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_AlbumPhoto_CreatedAtUtc");
            entity.Property(e => e.FilePath).HasMaxLength(260);
            entity.Property(e => e.OriginalFileName).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue((byte)1, "DF_AlbumPhoto_Status");
            entity.Property(e => e.UpdatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_AlbumPhoto_UpdatedAtUtc");

            entity.HasOne(d => d.Album).WithMany(p => p.AlbumPhotos)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AlbumPhoto_Album");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.HasIndex(e => e.AccountId, "UX_Employee_AccountId").IsUnique();

            entity.Property(e => e.EmployeeId).ValueGeneratedNever();
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Employee_CreatedAtUtc");
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.Status).HasDefaultValue((byte)1, "DF_Employee_Status");
            entity.Property(e => e.UpdatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Employee_UpdatedAtUtc");

            entity.HasOne(d => d.Account).WithOne(p => p.Employee)
                .HasForeignKey<Employee>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Account");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Member");

            entity.HasIndex(e => e.AccountId, "UX_Member_AccountId").IsUnique();

            entity.Property(e => e.MemberId).ValueGeneratedNever();
            entity.Property(e => e.AvatarPath).HasMaxLength(260);
            entity.Property(e => e.CreatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Member_CreatedAtUtc");
            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(320);
            entity.Property(e => e.Status).HasDefaultValue((byte)1, "DF_Member_Status");
            entity.Property(e => e.UpdatedAtUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Member_UpdatedAtUtc");

            entity.HasOne(d => d.Account).WithOne(p => p.Member)
                .HasForeignKey<Member>(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Member_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
