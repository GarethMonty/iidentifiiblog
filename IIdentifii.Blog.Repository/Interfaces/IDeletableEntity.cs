namespace IIdentifii.Blog.Repository
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }

        DateTime DeletedAt { get; set; }
    }
}
