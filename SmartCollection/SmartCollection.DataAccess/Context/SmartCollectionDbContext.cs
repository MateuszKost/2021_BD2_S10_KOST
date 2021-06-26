using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartCollection.Models.DBModels;

#nullable disable

namespace SmartCollection.DataAccess.Context
{
    public partial class SmartCollectionDbContext : IdentityDbContext
    {
        public SmartCollectionDbContext()
        {
        }

        public SmartCollectionDbContext(DbContextOptions<SmartCollectionDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<ImageDetail> ImageDetails { get; set; }
        public virtual DbSet<ImageAlbum> ImagesAlbums { get; set; }
        public virtual DbSet<ImageTag> ImagesTags { get; set; }
        public virtual DbSet<Privacy> Privacies { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        /*  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          {
              if (!optionsBuilder.IsConfigured)
              {
  #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                  optionsBuilder.UseNpgsql(*//*put connection string here*//*);
              }
          }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasAnnotation("Relational:Collation", "C.UTF-8");

            modelBuilder.Entity<Album>(entity =>
            {
                entity.ToTable("albums");

                entity.Property(e => e.AlbumId).HasColumnName("album_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasColumnType("character varying")
                    .HasColumnName("name");

                entity.Property(e => e.PrivacyId).HasColumnName("privacy_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Privacy)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.PrivacyId)
                    .HasConstraintName("albums_privacy_id_fkey");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("images");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.AlbumId).HasColumnName("album_id");

                entity.Property(e => e.ImageSha1)
                    .HasMaxLength(40)
                    .HasColumnName("image_sha1");

                entity.Property(e => e.UserId)
                    .HasMaxLength(40)
                    .HasColumnName("user_id");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("images_album_id_fkey");
            });

            modelBuilder.Entity<ImageDetail>(entity =>
            {
                entity.HasKey(e => e.ImageId)
                    .HasName("image_details_pkey");

                entity.ToTable("image_details");

                entity.Property(e => e.ImageId)
                    .ValueGeneratedNever()
                    .HasColumnName("image_id");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Height).HasColumnName("height");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");

                entity.Property(e => e.OriginalName)
                    .HasColumnType("character varying")
                    .HasColumnName("original_name");

                entity.Property(e => e.Width).HasColumnName("width");

                entity.HasOne(d => d.Image)
                    .WithOne(p => p.ImageDetail)
                    .HasForeignKey<ImageDetail>(d => d.ImageId)
                    .HasConstraintName("image_details_image_id_fkey");
            });

            modelBuilder.Entity<ImageAlbum>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => new { e.ImagesAlbumId, e.AlbumsAlbumId });

                entity.ToTable("images_albums");

                entity.Property(e => e.AlbumsAlbumId).HasColumnName("albums_album_id");

                entity.Property(e => e.ImagesAlbumId).HasColumnName("images_album_id");

                entity.HasOne(d => d.AlbumsAlbum)
                    .WithMany()
                    .HasForeignKey(d => d.AlbumsAlbumId)
                    .HasConstraintName("images_albums_albums_album_id_fkey");

                entity.HasOne(d => d.ImagesAlbumNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.ImagesAlbumId)
                    .HasConstraintName("images_albums_images_album_id_fkey");
            });

            modelBuilder.Entity<ImageTag>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => new { e.TagId,e.ImageId});

                entity.ToTable("images_tags");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.HasOne(d => d.Image)
                    .WithMany()
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("images_tags_image_id_fkey");

                entity.HasOne(d => d.Tag)
                    .WithMany()
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("images_tags_tag_id_fkey");
            });

            modelBuilder.Entity<Privacy>(entity =>
            {
                entity.ToTable("privacy");

                entity.Property(e => e.PrivacyId).HasColumnName("privacy_id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tags");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
