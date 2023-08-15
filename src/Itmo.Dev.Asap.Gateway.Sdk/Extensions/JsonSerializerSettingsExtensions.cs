using FluentSerialization;
using FluentSerialization.Extensions.NewtonsoftJson;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Tools;
using Newtonsoft.Json;

namespace Itmo.Dev.Asap.Gateway.Sdk.Extensions;

public static class JsonSerializerSettingsExtensions
{
    public static JsonSerializerSettings ApplyGatewaySerializationConfiguration(this JsonSerializerSettings serializerSettings)
    {
        ConfigurationBuilder
            .Build(new PresentationSerializationConfiguration())
            .ApplyToSerializationSettings(serializerSettings);

        return serializerSettings;
    }
}