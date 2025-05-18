using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomAuthenticationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var headers = context.HttpContext.Request.Headers;

        if (!headers.ContainsKey("Authorization") || headers["Authorization"] != "Bearer my-token")
        {
            context.Result = new UnauthorizedResult(); // Return 401 Unauthorized
            return;
        }

        // Custom logic to validate the token
        var token = headers["Authorization"].ToString().Replace("Bearer ", "");

        if (token != "my-token") // Replace this with your logic
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
