using System.ComponentModel.DataAnnotations;

namespace BlogEngine.Models
{
  public class CommentModel
  {
    [Required(AllowEmptyStrings = false)]
    public string Message { get; set; }
  }
}
