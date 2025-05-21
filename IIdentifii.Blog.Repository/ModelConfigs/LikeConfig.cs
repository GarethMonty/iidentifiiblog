using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IIdentifii.Blog.Repository
{
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
}
