namespace IIdentifii.Blog.Repository
{
    public class ReactionConfig : IEntityTypeConfiguration<ReactionModel>
    {
        public void Configure(
            EntityTypeBuilder<ReactionModel> builder)
        {
            builder.ToTable("Reactions");

            builder.HasKey(l => l.Id);

            builder.HasIndex(l => new { l.BlogPostId, l.Type })
                .IsUnique();

            builder.HasIndex(l => new { l.BlogPostId, l.UserId })
                .IsUnique();

            builder.Property(l => l.ReactedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(l => l.BlogPost)
                .WithMany(p => p.Reactions)
                .HasForeignKey(l => l.BlogPostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserId);
        }
    }
}
