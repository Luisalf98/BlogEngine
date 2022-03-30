using BlogEngine.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace BlogEngine.Authorization
{
  public class PostAuthorAuthorizationHandler : AuthorizationHandler<PostAuthorRequirement, Post>
  {
    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context, PostAuthorRequirement requirement, Post post
    )
    {
      if (context.User.Identity?.Name == post.Author.Username &&
          post.CurrentState == Post.State.Draft ||
          post.CurrentState == Post.State.Rejected)
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }

  public class PostAuthorRequirement : IAuthorizationRequirement { }
}
