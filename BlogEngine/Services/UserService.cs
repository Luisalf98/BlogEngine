using BlogEngine.Data;
using BlogEngine.Entities;
using BlogEngine.Models;
using BlogEngine.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace BlogEngine.Services
{
  public class UserService : BaseService
  {
    IConfiguration configuration;
    SigningCredentialsUtil signingCredentialsUtil;

    public UserService(AppDbContext context,
                       IConfiguration configuration,
                       SigningCredentialsUtil signingCredentialsUtil) : base(context)
    {
      this.configuration = configuration;
      this.signingCredentialsUtil = signingCredentialsUtil;
    }

    public User GetByUsername(string username, params string[] includes)
    {
      username = (username ?? "").Trim().ToUpper();
      IQueryable<User> users = context.Users;
      foreach(var relationship in includes)
      {
        users = users.Include(relationship);
      }

      return users.SingleOrDefault(u => u.Username.Trim().ToUpper() == username);
    }

    // Provisional return type
    public LoginResponseModel Login(string username, string password)
    {
      var user = GetByUsername(username, "Role");
      if (user == null)
        return new LoginResponseModel(false, "Invalid Username");

      if (!PasswordHasherUtil<User>.VerifyPassword(user, user.PasswordHash, password))
        return new LoginResponseModel(false, "Invalid password");

      var claims = new List<Claim>();
      claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
      claims.Add(new Claim(ClaimTypes.Name, username));

      try
      {
        var tokenJson = GenerateToken(claims);

        return new LoginResponseModel(true, "Login successful", tokenJson);
      }
      catch(Exception ex)
      {
        return new LoginResponseModel(false, ex.Message);
      }
    }

    private string GenerateToken(List<Claim> claims)
    {
      double expirationTimeInMinutes;
      if (!double.TryParse(configuration.GetSection("JWT")["ExpirationTimeInMinutes"], out expirationTimeInMinutes))
        throw new Exception("Invalid Expiration Time");

      var token = new JwtSecurityToken(
        configuration.GetSection("JWT")["Issuer"],
        configuration.GetSection("JWT")["Audience"],
        claims,
        DateTime.Now,
        DateTime.Now.AddMinutes(expirationTimeInMinutes),
        signingCredentialsUtil.SigningCredentials
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
