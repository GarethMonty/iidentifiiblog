namespace IIdentifii.Blog.Repository
{
    public class ReactionAggregateConfig : IEntityTypeConfiguration<ReactionAggregateModel>
    {
        public void Configure(
            EntityTypeBuilder<ReactionAggregateModel> builder)
        {
            builder.ToTable("ReactionAggregates");

            builder.HasKey(ra => ra.Id);

            builder.HasIndex(ra => new { ra.BlogPostId, ra.Type })
                .IsUnique();

            builder.HasOne(ra => ra.BlogPost)
                .WithMany(p => p.ReactionAggregates)
                .HasForeignKey(ra => ra.BlogPostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasQueryFilter(ra => ra.IsDeleted == false);
        }
    }
}
