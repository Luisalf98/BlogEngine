using System.ComponentModel.DataAnnotations;

namespace BlogEngine.Models
{
  public class PostModel
  {
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string Body { get; set; }
  }
}
