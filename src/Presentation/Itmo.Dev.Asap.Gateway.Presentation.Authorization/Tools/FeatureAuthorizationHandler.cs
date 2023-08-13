using Microsoft.AspNetCore.Authorization;

namespace Itmo.Dev.Asap.Gateway.Presentation.Authorization.Tools;

public class FeatureAuthorizationHandler : AuthorizationHandler<AuthorizationFeatureRequirement>
{
    private readonly IFeatureService _service;

    public FeatureAuthorizationHandler(IFeatureService service)
    {
        _service = service;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AuthorizationFeatureRequirement requirement)
    {
        if (_service.HasFeature(context.User, requirement.Scope, requirement.Feature))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}