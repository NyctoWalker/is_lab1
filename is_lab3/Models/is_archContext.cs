using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace is_lab3.Models
{
    public partial class is_archContext : DbContext
    {
        public is_archContext()
        {
        }

        public is_archContext(DbContextOptions<is_archContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PersonLicense> PersonLicenses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Gato1_otaG990;database=is_arch", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            modelBuilder.Entity<PersonLicense>(entity =>
            {
                entity.HasKey(e => e.IdCargoLicense)
                    .HasName("PRIMARY");

                entity.ToTable("person_license");

                entity.Property(e => e.IdCargoLicense).HasColumnName("id_cargo_license");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("first_name");

                entity.Property(e => e.HasDrivingLicense).HasColumnName("has_driving_license");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("last_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
