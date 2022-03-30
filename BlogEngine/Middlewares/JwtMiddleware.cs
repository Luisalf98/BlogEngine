using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogEngine.Middlewares
{
  public class JwtMiddleware
  {
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      string authHeader = context.Request.Headers["Authorization"];
      if (authHeader != null)
      {
        var jwtEncodedString = authHeader.Substring(7);

        var token = new JwtSecurityToken(jwtEncodedString: jwtEncodedString);

        var identity = new ClaimsIdentity(token.Claims, "basic");
        context.User = new ClaimsPrincipal(identity);
      }
      await _next(context);
    }
  }
}
