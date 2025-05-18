using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace API_11_01.Controllers
{
    public class CustomAuthFilter : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            // Example: Check for custom header "X-API-KEY"
            IEnumerable<string> apiKeyHeader;
            if (actionContext.Request.Headers.TryGetValues("X-API-KEY", out apiKeyHeader))
            {
                var apiKey = apiKeyHeader.FirstOrDefault();

                // You can replace this with real validation (e.g., from config or DB)
                if (apiKey == "my-secret-key")
                    return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(
                HttpStatusCode.Unauthorized,
                new { Message = "Unauthorized access. Invalid or missing API key." }
            );
        }
    }
}
