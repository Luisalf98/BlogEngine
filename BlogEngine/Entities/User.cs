using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogEngine.Entities
{
  [Index(nameof(Username), IsUnique = true)]
  public class User
  {
    public long Id { get; set; }
    [Required]
    public string Username { get; set; }
    [Required, JsonIgnore]
    public string PasswordHash { get; set; }
    [Required]
    public Role Role { get; set; }

    [JsonIgnore]
    public virtual IEnumerable<Comment> Comments { get; set; }
    [JsonIgnore]
    public virtual IEnumerable<Post> Posts { get; set; }
  }
}
