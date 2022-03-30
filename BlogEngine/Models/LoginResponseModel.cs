using System.Text.Json.Serialization;

namespace BlogEngine.Models
{
  public class LoginResponseModel
  {
    public bool Success { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string AccessToken { get; set; }
    public string Message { get; set; }

    public LoginResponseModel(bool success, string message, string accessToken = null)
    {
      Success = success;
      AccessToken = accessToken;
      Message = message;
    }
  }
}
