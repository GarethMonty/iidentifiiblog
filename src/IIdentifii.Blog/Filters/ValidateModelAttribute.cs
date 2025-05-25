using Microsoft.AspNetCore.Mvc.Filters;

namespace IIdentifii.Blog.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = context.ModelState
                    .Where(e => e.Value?.Errors?.Count > 0)
                    .Select(kvp => $"{kvp.Key}: {string.Join(',', kvp.Value?.Errors?.Select(a=> a.ErrorMessage).ToList()?? new List<string>())}")
                    .ToList();

                throw IIdentifiiException.Bad("Model Is Invalid", errors); 
            }
        }
    }
}
