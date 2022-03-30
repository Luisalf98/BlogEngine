using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlogEngine.Utilities
{
    public class SigningKeyUtil
    {
        public SecurityKey JwtSigningKey { get; }

        public SigningKeyUtil(IConfiguration configuration)
        {
            var secret = configuration.GetSection("JWT")["Secret"];
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            JwtSigningKey = new SymmetricSecurityKey(secretBytes);
        }
    }
}
