using MCBADataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MCBAAdminSite.Filters;

public class AuthorizeAdminAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Session.GetInt32("Admin").HasValue)
            context.Result = new RedirectToActionResult("Login", "Login", null);
    }
}
