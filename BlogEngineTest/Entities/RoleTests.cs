using BlogEngine.Data;
using BlogEngine.Entities;
using BlogEngineTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace BlogEngineTest.Entities
{
  public class RoleTests : IClassFixture<DatabaseFixture>, IDisposable
  {
    AppDbContext context;

    public RoleTests(DatabaseFixture fixture)
    {
      context = fixture.GetContext();
    }

    public void Dispose()
    {
      context.Dispose();
    }

    [Fact]
    public void NameShouldNotBeNull()
    {
      context.Add(new Role());

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void ValidRoleIsCreated()
    {
      context.Add(new Role { Name = "any_role_name" });

      context.SaveChanges();

      Assert.NotNull(context.Roles.SingleOrDefault(u => u.Name == "any_role_name"));
    }
  }
}
