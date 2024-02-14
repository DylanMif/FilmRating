using System;
using System.Collections.Generic;
using FilmRating.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace TP3Console.Models.EntityFramework
{
    public partial class FilmDBContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        public FilmDBContext()
        {
        }

        public FilmDBContext(DbContextOptions<FilmDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Utilisateur> Utilisateurs { get; set; } = null!;
        public virtual DbSet<Notation> Notations { get; set; } = null!;
        public virtual DbSet<Film> Films { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Notation>(entity =>
            {
                entity.HasKey(e => new { e.UtilisateurId, e.FilmId })
                    .HasName("pk_notations");

                entity.HasOne(n => n.UtilisateurNotant)
                    .WithMany(u => u.NotesUtilisateur)
                    .HasForeignKey(n => n.UtilisateurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_notation_utilisateur");

                entity.HasOne(n => n.FilmNote)
                    .WithMany(f => f.NotesFilm)
                    .HasForeignKey(n => n.FilmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_notation_film");
            });

            modelBuilder.Entity<Utilisateur>().Property(e => e.DateCreation).HasDefaultValueSql("now()");
            modelBuilder.Entity<Utilisateur>().Property(e => e.Pays).HasDefaultValue("France");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
