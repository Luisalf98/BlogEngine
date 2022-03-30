using BlogEngine.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace BlogEngineTest.Models
{
  public class CommentModelTests
  {
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("ANY COMMENT", true)]
    public void ShouldBeValidated(string message, bool isValid)
    {
      var commentModel = new CommentModel { Message = message };

      var ctx = new ValidationContext(commentModel);
      var res = Validator.TryValidateObject(commentModel, ctx, null);

      Assert.Equal(res, isValid);
    }
  }
}
