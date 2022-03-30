using Microsoft.AspNetCore.Mvc;
using BlogEngine.Services;
using BlogEngine.Models;
using Microsoft.AspNetCore.Http;

namespace BlogEngine.Controllers
{
  [ApiController]
  [Route("Api/[controller]")]
  public class UsersController : Controller
  {
    UserService userService;

    public UsersController(UserService userService)
    {
      this.userService = userService;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseModel))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Login([FromBody] UserModel model)
    {
      var result = userService.Login(model.Username, model.Password);
      if(result.Success)
        return Ok(result);

      return UnprocessableEntity(result);
    }

  }
}
