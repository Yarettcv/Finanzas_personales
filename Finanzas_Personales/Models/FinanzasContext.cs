using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Finanzas_Personales.Models;

public partial class FinanzasContext : DbContext
{
    public FinanzasContext()
    {
    }

    public FinanzasContext(DbContextOptions<FinanzasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoría> Categorías { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoría>(entity =>
        {
            entity.HasKey(e => e.IdCategoria);

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => e.IdMovimiento).HasName("PK_Movimientos_1");

            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Correo).HasName("PK_Usuarios_1");

            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdUsuarios).ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
