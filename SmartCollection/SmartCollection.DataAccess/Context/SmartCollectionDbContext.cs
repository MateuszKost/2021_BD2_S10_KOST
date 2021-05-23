using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SmartCollection.Models.DBModels;

#nullable disable

namespace SmartCollection.DataAccess.Context
{
    public partial class SmartCollectionDbContext : DbContext
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
        public virtual DbSet<ImagesAlbum> ImagesAlbums { get; set; }
        public virtual DbSet<Privacy> Privacies { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<TagOrder> TagOrders { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserCredential> UserCredentials { get; set; }
        public virtual DbSet<UsersAlbum> UsersAlbums { get; set; }

        //delete in future
/*        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql(*//*put here connection string*//*);

            }
        }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Albums)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("albums_user_id_fkey");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("images");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.AlbumId).HasColumnName("album_id");

                entity.Property(e => e.ImageSha1)
                    .HasMaxLength(40)
                    .HasColumnName("image_sha1");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("images_album_id_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("images_user_id_fkey");
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

            modelBuilder.Entity<ImagesAlbum>(entity =>
            {
                entity.HasNoKey();

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

            modelBuilder.Entity<Privacy>(entity =>
            {
                entity.ToTable("privacy");

                entity.Property(e => e.PrivacyId).HasColumnName("privacy_id");

                entity.Property(e => e.Name)
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

            modelBuilder.Entity<TagOrder>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tag_order");

                entity.Property(e => e.ImageId).HasColumnName("image_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.HasOne(d => d.Image)
                    .WithMany()
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("tag_order_image_id_fkey");

                entity.HasOne(d => d.Tag)
                    .WithMany()
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("tag_order_tag_id_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CredentialsId).HasColumnName("credentials_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<UserCredential>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("user_credentials_pkey");

                entity.ToTable("user_credentials");

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever()
                    .HasColumnName("user_id");

                entity.Property(e => e.Login)
                    .HasMaxLength(20)
                    .HasColumnName("login")
                    .IsFixedLength(true);

                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(40)
                    .HasColumnName("password_hash")
                    .IsFixedLength(true);

                entity.Property(e => e.Salt)
                    .HasColumnType("character varying")
                    .HasColumnName("salt");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserCredential)
                    .HasForeignKey<UserCredential>(d => d.UserId)
                    .HasConstraintName("user_credentials_user_id_fkey");
            });

            modelBuilder.Entity<UsersAlbum>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("users_albums");

                entity.Property(e => e.AlbumsUserId).HasColumnName("albums_user_id");

                entity.Property(e => e.UsersUserId).HasColumnName("users_user_id");

                entity.HasOne(d => d.AlbumsUser)
                    .WithMany()
                    .HasForeignKey(d => d.AlbumsUserId)
                    .HasConstraintName("users_albums_albums_user_id_fkey");

                entity.HasOne(d => d.UsersUser)
                    .WithMany()
                    .HasForeignKey(d => d.UsersUserId)
                    .HasConstraintName("users_albums_users_user_id_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
