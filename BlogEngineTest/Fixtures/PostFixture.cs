using BlogEngine.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlogEngineTest.Fixtures
{
  public class PostFixture : DatabaseFixture
  {
    public User User { get; }

    public PostFixture() : base()
    {
      using(var context = GetContext())
      {
        User = context.Users.SingleOrDefault(r => r.Username == "AUTHOR");
        if (User == null)
        {
          User = new User { 
            Role = new Role { Name = "AUTHOR_ROLE" },
            Username = "AUTHOR",
            PasswordHash = "ANY_PASSWORD"
          };
          context.Add(User);
          context.SaveChanges();
        }
      }
    }
  }
}
