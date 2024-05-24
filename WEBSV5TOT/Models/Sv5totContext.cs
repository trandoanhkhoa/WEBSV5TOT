using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WEBSV5TOT.Models;

public partial class Sv5totContext : DbContext
{
    public Sv5totContext()
    {
    }

    public Sv5totContext(DbContextOptions<Sv5totContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Part> Parts { get; set; }

    public virtual DbSet<ProofPicture> ProofPictures { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Student5Good> Student5Goods { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=SV5TOT;Integrated Security=True;TrustServerCertificate=True;");//"Data Source=LAPTOP-ENCKOU6S;Initial Catalog=SV5TOT;TrustServerCertificate=True;Integrated Security=True;TrustServerCertificate=True;"); //optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=SV5TOT;TrustServerCertificate=True;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FromDate)
                .HasColumnType("datetime")
                .HasColumnName("fromDate");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ToDate)
                .HasColumnType("datetime")
                .HasColumnName("toDate");
            entity.Property(e => e.Type).HasMaxLength(30);
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.ToTable("Class");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.ToTable("Part");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<ProofPicture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProofPic__3214EC2766E18A94");

            entity.ToTable("ProofPicture");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FileName).HasMaxLength(50);
            entity.Property(e => e.InputDate).HasColumnType("datetime");

            entity.HasOne(d => d.Student5Good).WithMany(p => p.ProofPictures)
                .HasForeignKey(d => d.Student5GoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProofPict__Stude__3C34F16F");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Student5Good>(entity =>
        {
            entity.ToTable("Student5Good");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Year).HasColumnType("date");

            entity.HasOne(d => d.Activity).WithMany(p => p.Student5Goods)
                .HasForeignKey(d => d.ActivityId)
                .HasConstraintName("FK_Student5Good_Activities");

            entity.HasOne(d => d.User).WithMany(p => p.Student5Goods)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Student5Good_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Avatar).HasMaxLength(100);
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(30);
            entity.Property(e => e.Mssv)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("MSSV");
            entity.Property(e => e.PartId).HasColumnName("PartID");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Permission)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.Users)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_Users_Class");

            entity.HasOne(d => d.Part).WithMany(p => p.Users)
                .HasForeignKey(d => d.PartId)
                .HasConstraintName("FK_Users_Part");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
