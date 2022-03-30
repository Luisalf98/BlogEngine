using Microsoft.AspNetCore.Mvc;
using BlogEngine.Entities;
using BlogEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Data;
using Microsoft.AspNetCore.Authorization;
using BlogEngine.Services;
using Microsoft.AspNetCore.Http;

namespace BlogEngine.Controllers
{
  [ApiController]
  [Route("Api/[controller]")]
  [Authorize]
  public class CommentsController : Controller
  {
    CommentService commentService;
    PostService postService;
    UserService userService;

    public CommentsController(
      CommentService commentService, 
      PostService postService,
      UserService userService
    )
    {
      this.commentService = commentService;
      this.postService = postService;
      this.userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IEnumerable<Comment> Index(long postId)
    {
      return commentService.GetAllByPostId(postId);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Comment))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Create([FromBody] CommentModel model, long postId)
    {
      if (!ModelState.IsValid)
        return UnprocessableEntity();

      var post = postService.GetAll().SingleOrDefault(p => p.Id == postId);
      if (post == null)
        return NotFound();

      var user = userService.GetByUsername(User.Identity.Name);
      try
      {
        var comment = commentService.CreateComment(model, user, post);

        return Ok(comment);
      }catch(Exception)
      {
        return UnprocessableEntity();
      }
    }
  }
}
