using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ChatApp.Infrastructure.Persistence;

public sealed class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var environmentName =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
            ?? "Development";
        var configuration = BuildConfiguration(environmentName);
        var connectionString = configuration.GetConnectionString("ChatAppDatabase")
            ?? throw new InvalidOperationException("Connection string 'ChatAppDatabase' was not found.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }

    private static IConfigurationRoot BuildConfiguration(string environmentName)
    {
        var configurationBasePath = ResolveConfigurationBasePath();

        return new ConfigurationBuilder()
            .SetBasePath(configurationBasePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
    }

    private static string ResolveConfigurationBasePath()
    {
        foreach (var candidatePath in GetConfigurationBasePaths())
        {
            if (File.Exists(Path.Combine(candidatePath, "appsettings.json")))
            {
                return candidatePath;
            }
        }

        throw new InvalidOperationException(
            "Could not locate appsettings.json for design-time DbContext creation.");
    }

    private static IEnumerable<string> GetConfigurationBasePaths()
    {
        var seenPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var seedPaths = new[]
        {
            Directory.GetCurrentDirectory(),
            AppContext.BaseDirectory
        };

        foreach (var seedPath in seedPaths)
        {
            foreach (var candidatePath in ExpandCandidatePaths(seedPath))
            {
                if (seenPaths.Add(candidatePath))
                {
                    yield return candidatePath;
                }
            }
        }
    }

    private static IEnumerable<string> ExpandCandidatePaths(string seedPath)
    {
        if (string.IsNullOrWhiteSpace(seedPath) || !Directory.Exists(seedPath))
        {
            yield break;
        }

        var current = new DirectoryInfo(Path.GetFullPath(seedPath));

        while (current is not null)
        {
            yield return current.FullName;

            var siblingApiPath = Path.Combine(current.FullName, "ChatApp.Api");
            if (Directory.Exists(siblingApiPath))
            {
                yield return siblingApiPath;
            }

            var srcApiPath = Path.Combine(current.FullName, "src", "ChatApp.Api");
            if (Directory.Exists(srcApiPath))
            {
                yield return srcApiPath;
            }

            current = current.Parent;
        }
    }
}
