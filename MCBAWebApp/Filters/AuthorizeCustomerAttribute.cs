using MCBADataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MCBAWebApp.Filters;

public class AuthorizeCustomerAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).HasValue)
            context.Result = new RedirectToActionResult("Index", "Home", null);
    }
}
