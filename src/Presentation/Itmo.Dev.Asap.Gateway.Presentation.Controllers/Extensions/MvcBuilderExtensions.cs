namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Extensions;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddPresentationControllers(this IMvcBuilder builder)
    {
        return builder.AddApplicationPart(typeof(IAssemblyMarker).Assembly);
    }
}