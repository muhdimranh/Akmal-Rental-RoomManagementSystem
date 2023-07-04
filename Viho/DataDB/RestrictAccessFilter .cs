using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Viho.web.DataDB
{
    public class RestrictAccessFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Check if the user has the required role ID
            if (user.Identity.IsAuthenticated && user.HasClaim("URoleid", "4"))
            {
                // Log out the user
                context.HttpContext.SignOutAsync();
                context.Result = new RedirectToActionResult("LoginWithImageTwo", "Authentication", null);
            }
        }
    }
}
