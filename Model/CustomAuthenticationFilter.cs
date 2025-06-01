using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomAuthenticationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Get the Authorization header
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

        // Simple validation
        if (string.IsNullOrEmpty(token) || token != "Bearer your-secret-token")
        {
            context.Result = new UnauthorizedResult(); // return 401
        }
    }
}
