﻿namespace IIdentifii.Blog.Repository
{
    public class CommentConfig : IEntityTypeConfiguration<CommentModel>
    {
        public void Configure(EntityTypeBuilder<CommentModel> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(c => c.Id);

            builder.HasIndex(ra => new { ra.BlogPostId, ra.UserId, ra.IsDeleted });

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(c => c.BlogPost)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.BlogPostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            builder
                .HasQueryFilter(x => x.IsDeleted == false);

        }
    }
}
