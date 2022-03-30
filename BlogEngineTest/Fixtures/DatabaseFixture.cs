using BlogEngine.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngineTest.Fixtures
{
  public class DatabaseFixture : IDisposable
  {
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AppDbContext> _options;

    public DatabaseFixture()
    {
      _connection = new SqliteConnection("datasource=:memory:");
      _connection.Open();

      _options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options;

      using (var context = GetContext())
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
      _connection.Close();
    }

    public AppDbContext GetContext()
    {
      return new AppDbContext(_options);
    }
  }
}
