using BlogEngine.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlogEngineTest.Fixtures
{
  public class UserFixture : DatabaseFixture
  {
    public Role Role { get; }

    public UserFixture() : base()
    {
      using(var context = GetContext())
      {
        Role = context.Roles.SingleOrDefault(r => r.Name == "ANY_ROLE");
        if (Role == null)
        {
          Role = new Role { Name = "ANY_ROLE" };
          context.Add(Role);
          context.SaveChanges();
        }
      }
    }
  }
}
