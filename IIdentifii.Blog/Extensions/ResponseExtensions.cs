using IIdentifii.Blog.Shared;
using Microsoft.AspNetCore.Mvc;

namespace IIdentifii.Blog.Extensions
{
    public static class ResponseExtensions
    {
        public static IActionResult ToResult<T>(this ApiResponse<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.Code
            };
        }
    }
}
