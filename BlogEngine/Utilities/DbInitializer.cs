using BlogEngine.Data;
using BlogEngine.Entities;
using System.Linq;

namespace BlogEngine.Utilities
{
  public static class DbInitializer
  {
    public static void Initialize(AppDbContext context)
    {
      context.Database.EnsureCreated();

      string[] roleNames = { "PUBLIC", "WRITER", "EDITOR" };
      foreach(var roleName in roleNames)
      {
        var role = context.Roles.SingleOrDefault(r => r.Name == roleName);
        if (role == null)
        {
          context.Add(new Role { Name = roleName });
        }
      }
      context.SaveChanges();

      foreach (var roleName in roleNames)
      {
        var user = context.Users.SingleOrDefault(r => r.Username == roleName);
        if (user == null)
        {
          user = new User {
            Username = roleName,
            Role = context.Roles.SingleOrDefault(r => r.Name == roleName)
          };
          user.PasswordHash = PasswordHasherUtil<User>.HashPassword(user, "password123");
          context.Add(user);
        }
      }
      context.SaveChanges();
    }
  }
}
