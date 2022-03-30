using Microsoft.AspNetCore.Mvc;
using BlogEngine.Entities;
using BlogEngine.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using BlogEngine.Services;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BlogEngine.Controllers
{
  [ApiController]
  [Route("Api/[controller]")]
  [Authorize]
  public class PostsController : Controller
  {
    PostService postService;
    UserService userService;

    private readonly IAuthorizationService _authorizationService;

    public PostsController(PostService postService, 
                          UserService userService,
                          IAuthorizationService authorizationService)
    {
      this.postService = postService;
      this.userService = userService;
      _authorizationService = authorizationService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IEnumerable<Post> Index()
    {
      return postService.GetAll().ToList();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public Post Get(long id)
    {
      return postService.GetAll().SingleOrDefault(p => p.Id == id);
    }

    [HttpGet("[action]")]
    [Authorize(Roles = "WRITER")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IEnumerable<Post> Mine()
    {
      var user = userService.GetByUsername(User.Identity.Name, "Posts");
      return user.Posts;
    }

    [HttpPost]
    [Authorize(Roles = "WRITER")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Create([FromBody] PostModel model)
    {
      if (!ModelState.IsValid)
        return UnprocessableEntity();

      var user = userService.GetByUsername(User.Identity.Name);
      try
      {
        var post = postService.CreatePost(model, user);

        return Ok(post);
      }
      catch (Exception)
      {
        return UnprocessableEntity();
      }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "WRITER")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update([FromBody] PostModel model, long id)
    {
      if (!ModelState.IsValid)
        return UnprocessableEntity();

      var post = postService.GetById(id, "Author");
      if (post == null)
        return NotFound();

      var authorizationResult = 
        await _authorizationService.AuthorizeAsync(User, post, "PostAuthorPolicy");

      if (!authorizationResult.Succeeded)
        return Forbid();

      try
      {
        postService.UpdatePost(post, model);

        return Ok(post);
      }
      catch (Exception)
      {
        return UnprocessableEntity();
      }
    }

    [HttpPatch("[action]/{id}")]
    [Authorize(Roles = "WRITER")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Submit(long id)
    {
      var post = postService.GetById(id, "Author");
      if (post == null)
        return NotFound();

      var authorizationResult =
        await _authorizationService.AuthorizeAsync(User, post, "PostAuthorPolicy");

      if (!authorizationResult.Succeeded)
        return Forbid();

      if (!postService.ExecuteAction(post, Post.Action.Submit))
        return UnprocessableEntity();

      return Ok(post);
    }

    [HttpPatch("[action]/{id}")]
    [Authorize(Roles = "EDITOR")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Reject(long id)
    {
      return ExecuteAction(Post.Action.Reject, id);
    }

    [HttpPatch("[action]/{id}")]
    [Authorize(Roles = "EDITOR")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Post))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Approve(long id)
    {
      return ExecuteAction(Post.Action.Approve, id);
    }

    private IActionResult ExecuteAction(Post.Action action, long id)
    {
      var post = postService.GetById(id);
      if (post == null)
        return NotFound();

      if (!postService.ExecuteAction(post, action))
        return UnprocessableEntity();

      return Ok(post);
    }
  }
}
