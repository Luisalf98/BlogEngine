using BlogEngine.Data;
using BlogEngine.Entities;
using BlogEngineTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace BlogEngineTest.Entities
{
  public class UserTests : IClassFixture<UserFixture>, IDisposable
  {
    AppDbContext context;
    Role role;

    public UserTests(UserFixture fixture)
    {
      context = fixture.GetContext();
      role = fixture.Role;
      context.Entry(role).State = EntityState.Unchanged;
    }

    public void Dispose()
    {
      context.Dispose();
    }

    [Fact]
    public void UsernameShouldNotBeNull()
    {
      context.Add(new User { PasswordHash = "anything", Role = role });

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void PasswordHashShouldNotBeNull()
    {
      context.Add(new User { Username = "any_username", Role = role });

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void RoleShouldNotBeNull()
    {
      context.Add(new User { Username = "any_username", PasswordHash = "anything" });

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void ValidUserIsCreated()
    {
      context.Add(new User { Username = "any_username", PasswordHash = "any_password_hash", Role = role });

      context.SaveChanges();

      Assert.NotNull(context.Users.SingleOrDefault(u => u.Username == "any_username"));
    }
  }
}
