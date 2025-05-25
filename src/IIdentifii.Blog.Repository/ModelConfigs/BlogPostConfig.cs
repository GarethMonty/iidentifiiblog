namespace IIdentifii.Blog.Repository
{
    public class BlogPostConfig : IEntityTypeConfiguration<BlogPostModel>
    {
        public void Configure(EntityTypeBuilder<BlogPostModel> builder)
        {
            builder.ToTable("BlogPosts");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(l => l.PostedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(p => p.Author)
                .WithMany(a => a.Posts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.PostedAt);

            builder
                .HasQueryFilter(p => p.IsDeleted == false);
        }
    }
}
