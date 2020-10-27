using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SP.DataManager.Models;

namespace SP.DataManager.Data
{
    public partial class SPDataContext : DbContext
    {
        public SPDataContext()
        {
        }

        public SPDataContext(DbContextOptions<SPDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DockManagers> DockManagers { get; set; }
        public virtual DbSet<Docks> Docks { get; set; }
        public virtual DbSet<Spaceships> Spaceships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SPData;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DockManagers>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First Name")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last Name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Docks>(entity =>
            {
                entity.Property(e => e.CurrentCapacity).HasColumnName("Current Capacity");

                entity.Property(e => e.MaxCapacity)
                    .HasColumnName("Max Capacity")
                    .HasDefaultValueSql("((6))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Docks)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dock_ToDockManager");
            });

            modelBuilder.Entity<Spaceships>(entity =>
            {
                entity.Property(e => e.CrewSize).HasColumnName("Crew Size");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Dock)
                    .WithMany(p => p.Spaceships)
                    .HasForeignKey(d => d.DockId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Spaceship_ToDock");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public List<DockManagers> GetDockManagers()
        {
            return DockManagers.ToList();
        }

        public 
    }
}
