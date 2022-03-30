using BlogEngine.Data;
using BlogEngine.Entities;
using BlogEngineTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace BlogEngineTest.Entities
{
  public class CommentTests : IClassFixture<CommentFixture>, IDisposable
  {
    AppDbContext context;
    User user;
    Post post;

    public CommentTests(CommentFixture fixture)
    {
      context = fixture.GetContext();
      user = fixture.User;
      post = fixture.Post;
      context.Entry(user).State = EntityState.Unchanged;
      context.Entry(post).State = EntityState.Unchanged;
    }

    public void Dispose()
    {
      context.Dispose();
    }

    [Fact]
    public void MessageShouldNotBeNull()
    {
      context.Add(new Comment { User = user, Post = post });

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void UserShouldNotBeNull()
    {
      context.Add(new Comment { Message = "ANY COMMENT", Post = post });

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void PostShouldNotBeNull()
    {
      context.Add(new Comment { Message = "ANY COMMENT", User = user });

      Assert.Throws<DbUpdateException>(() => context.SaveChanges());
    }

    [Fact]
    public void ValidCommentIsCreated()
    {
      var comment = new Comment { Message = "ANY COMMENT", User = user, Post = post };
      context.Add(comment);

      context.SaveChanges();

      Assert.True(context.Entry(comment).State == EntityState.Unchanged);
    }
  }
}
