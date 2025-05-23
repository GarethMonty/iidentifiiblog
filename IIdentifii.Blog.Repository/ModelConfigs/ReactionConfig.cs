namespace IIdentifii.Blog.Repository
{
    public class ReactionConfig : IEntityTypeConfiguration<ReactionModel>
    {
        public void Configure(
            EntityTypeBuilder<ReactionModel> builder)
        {
            builder.ToTable("Reactions");

            builder.HasKey(r => r.Id);

            builder.HasIndex(r => new { r.BlogPostId, r.Type })
                .IsUnique();

            builder.HasIndex(r => new { r.BlogPostId, r.UserId, r.IsDeleted })
                .IsUnique();

            builder.Property(r => r.ReactedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(r => r.BlogPost)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.BlogPostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);

            builder
                .HasQueryFilter(r => r.IsDeleted == false);

        }
    }
}
