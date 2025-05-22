namespace IIdentifii.Blog.Repository
{
    public class TagConfig : IEntityTypeConfiguration<TagModel>
    {
        public void Configure(EntityTypeBuilder<TagModel> builder)
        {
            builder.ToTable("Tags");

            builder.HasKey(mf => mf.Id);

            builder.Property(mf => mf.TaggedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(mf => mf.Type)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasIndex(mf => new { mf.BlogPostId, mf.ModeratorId, mf.Type })
                .IsUnique();

            builder.HasOne(mf => mf.BlogPost)
                .WithMany(p => p.Tags)
                .HasForeignKey(mf => mf.BlogPostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(mf => mf.Moderator)
                .WithMany()
                .HasForeignKey(mf => mf.ModeratorId);
        }
    }
}
