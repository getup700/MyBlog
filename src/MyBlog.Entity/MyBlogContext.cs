using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyBlog.EntityFrameworkCore.Models;

namespace MyBlog.EntityFrameworkCore;

public partial class MyBlogContext : DbContext
{
    public MyBlogContext()
    {
    }

    public MyBlogContext(DbContextOptions<MyBlogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlogNews> BlogNews { get; set; }

    public virtual DbSet<TypeInfo> TypeInfos { get; set; }

    public virtual DbSet<WriterInfo> WriterInfos { get; set; }

    public DbSet<Suggestion> Suggestions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        optionsBuilder.UseSqlServer("Server=.;Database=MyBlog;Trusted_Connection=True;TrustServerCertificate=True;");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlogNews>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_BlogNews_Id");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.Time).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(30);
        });

        modelBuilder.Entity<TypeInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_TypeInfo_Id");

            entity.ToTable("TypeInfo");

            entity.Property(e => e.Name).HasMaxLength(12);
        });

        modelBuilder.Entity<WriterInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_WriterInfo_Id");

            entity.ToTable("WriterInfo");

            entity.Property(e => e.Name).HasMaxLength(12);
            entity.Property(e => e.UserName).HasMaxLength(16);
            entity.Property(e => e.UserPwd).HasMaxLength(64);
        });

        modelBuilder.Entity<Suggestion>(entity =>
        {
            //entity.HasNoKey();
            entity.HasKey(e => e.Email).HasName("PK_Suggestion_Id");

            entity.ToTable("Suggestion");

            entity.Property(e => e.Name).HasMaxLength(12);
            entity.Property(e => e.Email).HasMaxLength(16);
            entity.Property(e => e.Content);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
