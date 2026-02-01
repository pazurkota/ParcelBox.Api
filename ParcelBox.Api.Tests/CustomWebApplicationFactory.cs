using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParcelBox.Api.Database;
using Testcontainers.PostgreSql;

namespace ParcelBox.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:18")
        .WithDatabase("testdb")
        .WithUsername("test")
        .WithPassword("test")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(_dbContainer.GetConnectionString());
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
    }
}