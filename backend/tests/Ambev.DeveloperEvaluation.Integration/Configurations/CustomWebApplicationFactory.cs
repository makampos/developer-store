using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Ambev.DeveloperEvaluation.Integration.Configurations;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly PostgreSqlContainer _postgresContainer;

    public CustomWebApplicationFactory()
    {
        _postgresContainer = new PostgreSqlBuilder()
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
    }

    protected override async void Dispose(bool disposing)
    {
        if (disposing)
        {
            await _postgresContainer.StopAsync();
        }

        base.Dispose(disposing);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<DefaultContext>(options =>
                options.UseNpgsql(_postgresContainer.GetConnectionString()));

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();
            dbContext.Database.Migrate();
        });
    }
}