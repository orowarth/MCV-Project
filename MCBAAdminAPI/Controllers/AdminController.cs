using MCBAAdminAPI.Communication;
using Microsoft.AspNetCore.Mvc;

namespace MCBAAdminAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    [HttpPost("Login")]
    public IActionResult Login(LoginDto loginDto)
    {
        if (loginDto.LoginID == "admin" && loginDto.Password == "admin")
        {
            return Ok();
        }

        return Unauthorized();
    }
}