using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL;

/// <summary>
/// A factory class for creating instances of the <see cref="AppDbContext"/> at design time.
/// This is typically used by Entity Framework Core tools to create the DbContext during design-time operations, 
/// such as migrations or scaffolding.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Creates and configures an instance of the <see cref="AppDbContext"/> class at design time.
    /// </summary>
    /// <param name="args">An array of command-line arguments. This parameter is not used in this implementation.</param>
    /// <returns>An instance of the <see cref="AppDbContext"/> configured with SQLite connection settings.</returns>
    /// <remarks>
    /// This method is called by Entity Framework Core tools when running commands such as migrations.
    /// It sets up the DbContext with the SQLite connection string and enables detailed errors and sensitive data logging.
    /// </remarks>
    public AppDbContext CreateDbContext(string[] args)
    {
        // Define the connection string, using a SQLite database located at the specified base path
        var connectionString = $"Data Source={FileHelper.BasePath}app.db";
        
        // Configure the DbContext options with SQLite, detailed error messages, and sensitive data logging
        var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connectionString)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .Options;
        
        // Return a new instance of AppDbContext with the configured options
        return new AppDbContext(contextOptions);
    }
}