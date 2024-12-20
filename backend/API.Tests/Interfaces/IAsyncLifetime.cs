namespace API.Tests.Interfaces;

public interface IAsyncLifetime
{
    Task InitializeAsync();
    Task DisposeAsync();
}