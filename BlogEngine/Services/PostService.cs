using BlogEngine.Data;
using BlogEngine.Entities;
using BlogEngine.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BlogEngine.Services
{
  public class PostService : BaseService
  {
    public PostService(AppDbContext _) : base(_) { }

    public Post GetById(long postId, params string[] includes)
    {
      IQueryable<Post> posts = context.Posts;
      foreach (var relationship in includes)
      {
        posts = posts.Include(relationship);
      }

      return posts.SingleOrDefault(p => p.Id == postId);
    }

    public IQueryable<Post> GetAll()
    {
      return context.Posts.Where(p => p.CurrentState == Post.State.Approved);
    }

    public Post CreatePost(PostModel postModel, User user)
    {
      var post = new Post
      {
        Title = postModel.Title.Trim(),
        Body = postModel.Body.Trim(),
        AuthorId = user.Id
      };

      context.Add(post);

      context.SaveChanges();

      return post;
    }

    public void UpdatePost(Post post, PostModel postModel)
    {
      post.Title = postModel.Title.Trim();
      post.Body = postModel.Body.Trim();

      context.SaveChanges();
    }

    public bool ExecuteAction(Post post, Post.Action action)
    {
      if (!post.TakeAction(action))
        return false;

      context.SaveChanges();

      return true;
    }
  }
}
