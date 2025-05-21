namespace IIdentifii.Blog.Repository
{
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
}
