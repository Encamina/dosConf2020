using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AzureCognitiveSearch.Shared.Utils.Extensions
{
    public static class ActionContextExtensions
    {
        public static BadRequestObjectResult ValidationError(this ActionContext context)
        {
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Instance = context.HttpContext.Request.Path,
                Status = (int)HttpStatusCode.BadRequest,
                Detail = "Invalid input data. See additional details in 'errors' property."
            };
            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json", "application/problem+xml" }
            };
        }
    }
}
