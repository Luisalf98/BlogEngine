using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using BlogEngine.Data;
using BlogEngine.Utilities;

namespace BlogEngine
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = CreateWebHostBuilder(args).Build();
      CreateDatabaseIfNotExists(host);
      host.Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args)
    {
      return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
        
    public static void CreateDatabaseIfNotExists(IWebHost host)
    {
      using (var scope = host.Services.CreateScope())
      {
        try
        {
          var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
          DbInitializer.Initialize(context);
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }
  }
}
