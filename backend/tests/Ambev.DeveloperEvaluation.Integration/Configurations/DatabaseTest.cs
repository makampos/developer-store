using Ambev.DeveloperEvaluation.ORM;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Configurations;

public class DatabaseTest : IAsyncLifetime
{
    protected HttpClient Client;
    protected readonly DefaultContext DbContext;
    private readonly Func<Task> _resetDatabase;

    protected DatabaseTest(IntegrationTestFactory factory)
    {
        _resetDatabase = factory.ResetDatabase;
        DbContext = factory.DefaultContext;
        Client = factory.CreateClient();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;
}