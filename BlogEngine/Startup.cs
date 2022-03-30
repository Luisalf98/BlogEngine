using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using BlogEngine.Data;
using BlogEngine.Utilities;
using BlogEngine.Authorization;
using BlogEngine.Services;
using BlogEngine.Middlewares;

namespace BlogEngine
{
  public class Startup
  {

    private IConfiguration Configuration;

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {

      var signingKeyUtil = new SigningKeyUtil(Configuration);
      services.AddSingleton(signingKeyUtil);
      var signingCredentialsUtil = new SigningCredentialsUtil(signingKeyUtil);
      services.AddSingleton(signingCredentialsUtil);

      services.AddDbContext<AppDbContext>(config => {
        config.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
      });
      services.AddScoped<CommentService>();
      services.AddScoped<PostService>();
      services.AddScoped<UserService>();

      services.AddSingleton<IAuthorizationHandler, PostAuthorAuthorizationHandler>();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                configureOptions => {
                  configureOptions.TokenValidationParameters = new TokenValidationParameters
                  {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetSection("JWT")["Issuer"],
                    ValidAudience = Configuration.GetSection("JWT")["Audience"],
                    IssuerSigningKey = signingKeyUtil.JwtSigningKey
                  };
                }
              );

      services.AddAuthorization(configure => {
        configure.DefaultPolicy = 
          new AuthorizationPolicyBuilder(
            JwtBearerDefaults.AuthenticationScheme
          ).RequireAuthenticatedUser()
           .Build();

        configure.AddPolicy(
          "PostAuthorPolicy",
          policy => policy.Requirements.Add(new PostAuthorRequirement())
        );
      });

      services.AddControllers();

      services.AddSwaggerDocument(settings =>
      {
        settings.Title = "Blog Engine APIDOC";
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.EnvironmentName == "Development")
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseOpenApi();
      app.UseSwaggerUi3();

      app.UseCors(configurePolicy =>
      {
        configurePolicy.AllowAnyOrigin();
      });

      app.UseRouting();
      app.UseAuthentication();
      app.UseMiddleware<JwtMiddleware>();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute("API", "api/{controller}/{action}/{id?}");
      });
    }
  }
}
