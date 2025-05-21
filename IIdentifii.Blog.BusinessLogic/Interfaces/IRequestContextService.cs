
namespace IIdentifii.Blog.BusinessLogic
{
    public interface IRequestContextService
    {
        string? UserId { get; }

        string? UserName { get; }

        bool TryGetUserId(out Guid userId);
    }
}