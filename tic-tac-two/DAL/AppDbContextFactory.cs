using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Creates and configures an instance of the <see cref="AppDbContext"/> class at design time.
    /// </summary>
    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = "Data Source=<%location%>app.db";
        connectionString = connectionString.Replace("<%location%>", FileHelper.BasePath);
            //$"Data Source={FileHelper.BasePath}app.db";
            
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;
        
        return new AppDbContext(contextOptions);
    }
}