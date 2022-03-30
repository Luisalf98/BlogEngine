using BlogEngine.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlogEngineTest.Fixtures
{
  public class CommentFixture : DatabaseFixture
  {
    public User User { get; }
    public Post Post { get; }

    public CommentFixture() : base()
    {
      using(var context = GetContext())
      {
        User = context.Users.SingleOrDefault(r => r.Username == "ANY_USER");
        if (User == null)
        {
          User = new User { 
            Role = new Role { Name = "AUTHOR_ROLE" },
            Username = "ANY_USER",
            PasswordHash = "ANY_PASSWORD_HASH"
          };
          context.Add(User);
          context.SaveChanges();
        }

        Post = context.Posts.FirstOrDefault(r => r.AuthorId == User.Id);
        if (Post == null)
        {
          Post = new Post
          {
            Author = User,
            Title = "ANY TITLE",
            Body = "ANY BODY"
          };
          context.Add(Post);
          context.SaveChanges();
        }
      }
    }
  }
}
