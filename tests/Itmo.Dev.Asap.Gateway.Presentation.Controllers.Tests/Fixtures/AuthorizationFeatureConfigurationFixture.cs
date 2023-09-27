using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization.Models;
using Microsoft.Extensions.Configuration;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Tests.Fixtures;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class AuthorizationFeatureConfigurationFixture
{
    internal AuthorizationConfiguration Configuration { get; }

    public AuthorizationFeatureConfigurationFixture()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("features.json")
            .Build();

        Configuration = configuration.GetSection(Constants.SectionKey).Get<AuthorizationConfiguration>()
                        ?? throw new InvalidOperationException("Failed to parse features.json");
    }
}