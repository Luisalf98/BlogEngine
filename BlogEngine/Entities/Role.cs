using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogEngine.Entities
{
  [Index(nameof(Name), IsUnique = true)]
  public class Role
  {
    public long Id { get; set; }
    [Required]
    public string Name { get; set; }

    [JsonIgnore]
    public virtual IEnumerable<User> Users { get; set; }
  }
}
