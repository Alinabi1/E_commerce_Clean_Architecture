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
            var databaseName = $"user_service_test_{Guid.NewGuid():N}";

            var connectionString =
                $"Server=mysql;Database={databaseName};User=root;Password=root;";

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26)),
                    mySqlOptions => mySqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null))
                .Options;

            int retry = 0;
            int maxRetry = 10;
            while (true)
            {
                try
                {
                    var dbContext = new AppDbContext(options);
                    dbContext.Database.EnsureCreated(); // här försöker EF ansluta
                    Console.WriteLine($"Creating DB for test: {databaseName}");
                    return dbContext;
                }
                catch (MySqlConnector.MySqlException)
                {
                    retry++;
                    if (retry >= maxRetry)
                        throw; // ger upp efter 10 försök
                    Console.WriteLine($"Waiting for MySQL to start... Attempt {retry}/{maxRetry}");
                    Task.Delay(2000).Wait(); // vänta 2 sek och försök igen
                }
            }
        }

        protected void CleanupDatabase(AppDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
