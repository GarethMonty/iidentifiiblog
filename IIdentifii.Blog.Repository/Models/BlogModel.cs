using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IIdentifii.Blog.Repository
{
    public record BlogPostModel
    {
        #region Properties

        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public DateTime PublishedAt { get; set; }

        public bool IsPublished { get; set; }

        public Guid AuthorId { get; set; }

        public IIdentifiiUser Author { get; set; } = null!;

        public ICollection<LikeModel> Likes { get; set; } = new List<LikeModel>();

        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();

        public ICollection<ModeratorFlagModel> ModeratorFlags { get; set; } = new List<ModeratorFlagModel>();

        #endregion
    }

    public record AuthorModel
    {
        #region Properties

        public Guid Id { get; set; }

        public string DisplayName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Bio { get; set; }

        public ICollection<BlogPostModel> Posts { get; set; } = new List<BlogPostModel>();

        #endregion
    }

    public record LikeModel
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid BlogPostId { get; set; }

        public BlogPostModel BlogPost { get; set; } = null!;

        public Guid UserId { get; set; }

        public IIdentifiiUser User { get; set; } = null!;

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        #endregion
    }

    public record CommentModel
    {
        #region Properties

        public Guid Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public Guid BlogPostId { get; set; }

        public BlogPostModel BlogPost { get; set; } = null!;

        public Guid UserId { get; set; }

        public IIdentifiiUser User { get; set; } = null!;

        #endregion
    }

    public class ModeratorFlagModel
    {
        #region Properties

        public Guid Id { get; set; }

        public Guid BlogPostId { get; set; }

        public BlogPostModel BlogPost { get; set; } = null!;

        public Guid ModeratorId { get; set; }

        public IIdentifiiUser Moderator { get; set; } = null!;

        public DateTime FlaggedAt { get; set; } = DateTime.UtcNow;

        public string Reason { get; set; } = "Misleading or false information";

        #endregion
    }

    public class BlogPostConfig : IEntityTypeConfiguration<BlogPostModel>
    {
        public void Configure(EntityTypeBuilder<BlogPostModel> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(p => p.PublishedAt)
                .IsRequired();

            builder.Property(p => p.IsPublished)
                .IsRequired();

            builder.HasOne(p => p.Author)
                .WithMany(a => a.Posts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.PublishedAt);
        }
    }

    public class LikeConfig : IEntityTypeConfiguration<LikeModel>
    {
        public void Configure(EntityTypeBuilder<LikeModel> builder)
        {
            builder.HasKey(l => l.Id);

            builder.HasIndex(l => new { l.BlogPostId, l.UserId })
                .IsUnique();

            builder.Property(l => l.LikedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(l => l.BlogPost)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.BlogPostId);

            builder.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId);
        }
    }

    public class CommentConfig : IEntityTypeConfiguration<CommentModel>
    {
        public void Configure(EntityTypeBuilder<CommentModel> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(c => c.BlogPost)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.BlogPostId);

            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);
        }
    }

    public class ModeratorFlagConfig : IEntityTypeConfiguration<ModeratorFlagModel>
    {
        public void Configure(EntityTypeBuilder<ModeratorFlagModel> builder)
        {
            builder.HasKey(mf => mf.Id);

            builder.Property(mf => mf.FlaggedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(mf => mf.Reason)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasIndex(mf => new { mf.BlogPostId, mf.ModeratorId })
                .IsUnique();

            builder.HasOne(mf => mf.BlogPost)
                .WithMany(p => p.ModeratorFlags)
                .HasForeignKey(mf => mf.BlogPostId);

            builder.HasOne(mf => mf.Moderator)
                .WithMany()
                .HasForeignKey(mf => mf.ModeratorId);
        }
    }

    public class AuthorConfig : IEntityTypeConfiguration<AuthorModel>
    {
        public void Configure(EntityTypeBuilder<AuthorModel> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.DisplayName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.Email)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.HasIndex(a => a.Email)
                   .IsUnique();

            builder.Property(a => a.Bio)
                   .HasMaxLength(500);

            builder.HasMany(a => a.Posts)
                   .WithOne(p => p.Author)
                   .HasForeignKey(p => p.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
