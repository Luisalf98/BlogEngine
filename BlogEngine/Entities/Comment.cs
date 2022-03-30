using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogEngine.Entities
{
  public class Comment
  {
    public long Id { get; set; }
    [Required]
    public string Message { get; set; }
    [Required]
    public long PostId { get; set; }
    [Required]
    public long UserId { get; set; }
    
    public Post Post { get; set; }
    public User User { get; set; }
  }
}
