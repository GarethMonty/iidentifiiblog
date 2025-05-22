using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement.Mvc;

namespace IIdentifii.Blog
{
    public class ApiFeatureNotAvailableHandler : IDisabledFeaturesHandler
    {
        public Task HandleDisabledFeatures(
            IEnumerable<string> features, 
            ActionExecutingContext context)
        {
            ApiResponse<object> result = new ApiResponse<object>
            {
                IsSuccess = false,
                Code = StatusCodes.Status501NotImplemented,
                ErrorMessage = "Feature not available",
                Data = null
            };

            context.Result = result.ToResult();

            return Task.CompletedTask;
        }
    }
}
