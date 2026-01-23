using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Data;

namespace IntegrationsTests
{
    public abstract class IntegrationTestBase
    {
        protected AppDbContext CreateDbContext()
        {
            Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            var databaseName = $"user_service_test_{Guid.NewGuid():N}";

            var connectionString =
                $"Server=mysql;Database={databaseName};User=root;Password=root;";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString,
                    new MySqlServerVersion(new Version(8, 0, 26)))
                .Options;

            var dbContext = new AppDbContext(options);
            dbContext.Database.EnsureCreated();
            Console.WriteLine($"Creating DB for test  : {databaseName}");

            return dbContext;
        }

        protected void CleanupDatabase(AppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
