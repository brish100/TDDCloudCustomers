using Microsoft.AspNetCore.Mvc;

namespace CloudCustomers.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase {
    private readonly IUsersService _usersService;

    public UsersController(IUsersService iUsersService)
    {
        _usersService = iUsersService;
    }

    [HttpGet(Name = "GetUsers")]
    public async Task<IActionResult> Get()
    {
        var users = await _usersService.GetAllUsers();
        return Ok(users);
    }
}