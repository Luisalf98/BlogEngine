using Microsoft.IdentityModel.Tokens;

namespace BlogEngine.Utilities
{
  public class SigningCredentialsUtil
  {
    public SigningCredentials SigningCredentials { get; }

    public SigningCredentialsUtil(SigningKeyUtil signingKeyUtil)
    {
      SigningCredentials = new SigningCredentials(signingKeyUtil.JwtSigningKey, SecurityAlgorithms.HmacSha256);
    }
  }
}
