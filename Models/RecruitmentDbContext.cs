using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RecruitmentApplication.Models;

public partial class RecruitmentDbContext : DbContext
{
    public RecruitmentDbContext()
    {
    }

    public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Offre> Offres { get; set; }

    public virtual DbSet<Recruteur> Recruteurs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:RecruitmentCS");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Offre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Offre__3214EC079A126918");

            entity.ToTable("Offre");

            entity.Property(e => e.RecruteurId).HasColumnName("Recruteur_Id");

            entity.HasOne(d => d.Recruteur).WithMany(p => p.Offres)
                .HasForeignKey(d => d.RecruteurId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_recruteurId");
        });

        modelBuilder.Entity<Recruteur>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Recruteu__3214EC077371BBC2");

            entity.ToTable("Recruteur");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC0728B44D49");

            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Role).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
