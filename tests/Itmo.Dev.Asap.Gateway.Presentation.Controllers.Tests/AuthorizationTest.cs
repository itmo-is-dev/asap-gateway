using FluentAssertions;
using FluentScanning;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Tests.TheoryData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Text;
using Xunit;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Tests;

public class AuthorizationTest
{
    [Theory]
    [ClassData(typeof(ControllersClassesTestData))]
    public void ControllersShouldNotHaveAuthorizeAttribute(Type assemblyMarker)
    {
        var scanner = new AssemblyScanner(assemblyMarker.Assembly);

        scanner.ScanForTypesThat()
            .AreAssignableTo<ControllerBase>()
            .Should()
            .NotContain(controller => controller.CustomAttributes
                .Any(attrib => attrib.AttributeType.IsAssignableTo(typeof(AuthorizeAttribute))));
    }

    [Theory]
    [ClassData(typeof(ControllersClassesTestData))]
    public void ControllerActionsShouldHaveAuthorizeAttribute(Type assemblyMarker)
    {
        var scanner = new AssemblyScanner(assemblyMarker.Assembly);
        TypeInfo[] controllers = scanner.ScanForTypesThat().AreAssignableTo<ControllerBase>().ToArray();

        foreach (TypeInfo controller in controllers)
        {
            IEnumerable<MethodInfo> actions = controller
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(method => method.CustomAttributes.Any(attrib => attrib.AttributeType.IsAssignableTo(typeof(IRouteTemplateProvider))))
                .Where(method => method.GetCustomAttribute<AllowAnonymousAttribute>() is null);

            MethodInfo[] actionWithoutAuthorization = actions
                .Where(action => action.CustomAttributes.All(attrib =>
                    attrib.AttributeType.IsAssignableTo(typeof(AuthorizeAttribute)) is false))
                .ToArray();

            StringBuilder becauseMessageBuilder = actionWithoutAuthorization
                .Select(action => $"{action.DeclaringType?.Name}.{action.Name}")
                .Aggregate(
                    new StringBuilder("Following methods must explicitly specify AuthorizationAttribute:\n"),
                    (builder, actionName) => builder.Append('\t').AppendLine(actionName));

            actionWithoutAuthorization.Should().BeEmpty(becauseMessageBuilder.ToString());
        }
    }
}