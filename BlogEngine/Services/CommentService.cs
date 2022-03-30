using BlogEngine.Data;
using BlogEngine.Entities;
using BlogEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogEngine.Services
{
  public class CommentService : BaseService
  {
    public CommentService(AppDbContext _) : base(_) { }

    public IEnumerable<Comment> GetAllByPostId(long postId)
    {
      return context.Comments.Where(c => c.PostId == postId).ToList();
    }

    public Comment CreateComment(
      CommentModel commentModel, User user, Post post  
    )
    {
      var comment = new Comment
      {
        Message = commentModel.Message.Trim(),
        UserId = user.Id,
        PostId = post.Id
      };

      context.Add(comment);

      context.SaveChanges();

      return comment;
    }
  }
}
