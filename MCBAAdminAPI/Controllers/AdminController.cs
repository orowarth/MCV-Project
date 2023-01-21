using MCBADataLibrary.Admin.Communication;
using Microsoft.AspNetCore.Mvc;

namespace MCBAAdminAPI.Controllers;


/// <summary>
/// <c>AdminController</c> handles requests related to admin-related tasks, such as an admin logging in.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [HttpPost("Login")]
    public IActionResult Login(LoginInput loginDto)
    {
        if (loginDto.LoginID == "admin" && loginDto.Password == "admin")
        {
            return Ok();
        }

        return Unauthorized();
    }
}