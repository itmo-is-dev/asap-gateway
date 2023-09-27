using FluentAssertions;
using FluentScanning;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization.Models;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Tests.Fixtures;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Tests.TheoryData;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Xunit;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Tests;

public class AuthorizeFeatureTest : IClassFixture<AuthorizationFeatureConfigurationFixture>
{
    private readonly AuthorizationConfiguration _configuration;

    public AuthorizeFeatureTest(AuthorizationFeatureConfigurationFixture fixture)
    {
        _configuration = fixture.Configuration;
    }

    [Theory]
    [ClassData(typeof(ControllersClassesTestData))]
    public void FeatureScopesShouldBeDefined(Type assemblyMarker)
    {
        var scanner = new AssemblyScanner(assemblyMarker.Assembly);
        IScanningQuery controllers = scanner.ScanForTypesThat().AreAssignableTo<ControllerBase>();

        IEnumerable<AuthorizeFeatureAttribute> features = controllers
            .SelectMany(controller => controller.GetMethods(BindingFlags.Instance | BindingFlags.Public))
            .Select(method => method.GetCustomAttribute<AuthorizeFeatureAttribute>())
            .Where(method => method is not null)
            .Select(method => method!);

        foreach (AuthorizeFeatureAttribute feature in features)
        {
            _configuration
                .FeatureScopes.Should().ContainKey(feature.Scope)
                .WhoseValue.Should().ContainKey(feature.Feature)
                .WhoseValue.Should().NotBeEmpty();
        }
    }
}