using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace is_lab6.Models
{
    public partial class is_arch6Context : DbContext
    {
        public is_arch6Context()
        {
        }

        public is_arch6Context(DbContextOptions<is_arch6Context> options)
            : base(options)
        {
        }

        public virtual DbSet<ShopItem> ShopItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=Gato1_otaG990;database=is_arch6", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            modelBuilder.Entity<ShopItem>(entity =>
            {
                entity.HasKey(e => e.ItemId)
                    .HasName("PRIMARY");

                entity.ToTable("shop_items");

                entity.HasIndex(e => e.ItemName, "item_name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.ItemName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("item_name");

                entity.Property(e => e.ItemPrice).HasColumnName("item_price");

                entity.Property(e => e.ItemStats)
                    .HasMaxLength(250)
                    .HasColumnName("item_stats");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
