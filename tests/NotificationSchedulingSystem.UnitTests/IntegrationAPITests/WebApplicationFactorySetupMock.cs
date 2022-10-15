using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NotificationSchedulingSystem.Business.Services;


namespace NotificationSchedulingSystem.UnitTests.IntegrationAPITests;

public class WebApplicationFactorySetupMock : IDisposable
{
    private readonly WebApplicationFactory<Program> _factory = new();
    private readonly CompanyServiceMock _companyService = new();
    private HttpClient? _client;
    public HttpClient Setup()
    {
        _companyService.Setup();
        _client = _factory.WithWebHostBuilder(
                builder => builder.ConfigureTestServices(
                    services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                 typeof(ICompanyService));

                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        services.AddTransient(_ => _companyService.Object);
                    }))
            .CreateClient();

        return _client;
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client?.Dispose();
    }
}

[CollectionDefinition("WebApplicationFactory collection")]
public class DatabaseCollection : ICollectionFixture<WebApplicationFactorySetupMock>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}