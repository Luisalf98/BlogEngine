using BlogEngine.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BlogEngineTest.Models
{
  public class PostModelTests
  {
    [Theory]
    [InlineData(null, null, false)]
    [InlineData("", "", false)]
    [InlineData("", "ANY BODY", false)]
    [InlineData("ANY TITLE", "", false)]
    [InlineData(null, "ANY BODY", false)]
    [InlineData("ANY TITLE", null, false)]
    [InlineData("ANY TITLE", "ANY BODY", true)]
    public void ShouldBeValidated(string title, string body, bool isValid)
    {
      var commentModel = new PostModel { Title = title, Body = body };

      var ctx = new ValidationContext(commentModel);
      var res = Validator.TryValidateObject(commentModel, ctx, null);

      Assert.Equal(res, isValid);
    }
  }
}
