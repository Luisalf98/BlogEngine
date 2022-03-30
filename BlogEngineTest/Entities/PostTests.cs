using BlogEngine.Data;
using BlogEngine.Entities;
using BlogEngineTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace BlogEngineTest.Entities
{
  public class PostTests : IClassFixture<PostFixture>, IDisposable
  {
    AppDbContext context;
    User user;

    public PostTests(PostFixture fixture)
    {
      context = fixture.GetContext();
      user = fixture.User;
      context.Entry(user).State = EntityState.Unchanged;
    }

    public void Dispose()
    {
      context.Dispose();
    }

    [Fact]
    public void TitleShouldNotBeNull()
    {
      context.Add(new Post { Author = user, Body = "ANY LONG TEXT" });

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void BodyShouldNotBeNull()
    {
      context.Add(new Post { Author = user, Title = "ANY TITLE" });

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void AuthorShouldNotBeNull()
    {
      context.Add(new Post { Title = "ANY TITLE", Body = "ANY LONG TEXT" });

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void ValidPostIsCreated()
    {
      var post = new Post { Title = "ANY TITLE", Body = "ANY LONG TEXT", Author = user };
      context.Add(post);

      context.SaveChanges();

      Assert.True(context.Entry(post).State == EntityState.Unchanged);
    }

    [Fact]
    public void ValidPostIsCreatedWithDefaultState()
    {
      var post = new Post { Title = "ANY TITLE", Body = "ANY LONG TEXT", Author = user };
      context.Add(post);

      context.SaveChanges();

      Assert.True(post.CurrentState == Post.State.Draft);
    }
  }
}
