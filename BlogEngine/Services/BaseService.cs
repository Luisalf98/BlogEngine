using BlogEngine.Data;
using System;

namespace BlogEngine.Services
{
  public abstract class BaseService : IDisposable
  {
    public AppDbContext context { get; set; }

    public BaseService(AppDbContext context)
    {
      this.context = context;
    }

    public void Dispose()
    {
      context.Dispose();
    }
  }
}
