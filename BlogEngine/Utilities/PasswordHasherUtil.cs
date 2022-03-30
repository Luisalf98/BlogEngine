using Microsoft.AspNetCore.Identity;

namespace BlogEngine.Utilities
{
  public static class PasswordHasherUtil<T> where T : class
  {
    private static PasswordHasher<T> passwordHasher = new PasswordHasher<T>();

    public static string HashPassword(T t, string password)
    {
      return passwordHasher.HashPassword(t, password);
    }

    public static bool VerifyPassword(T t, string hashedPassword, string password)
    {
      return passwordHasher.VerifyHashedPassword(t, hashedPassword, password) == 
             PasswordVerificationResult.Success;
    }
  }
}
