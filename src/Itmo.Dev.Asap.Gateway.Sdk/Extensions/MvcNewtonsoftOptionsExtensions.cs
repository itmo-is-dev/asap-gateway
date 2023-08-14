using FluentSerialization;
using FluentSerialization.Extensions.NewtonsoftJson;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Itmo.Dev.Asap.Gateway.Sdk.Extensions;

public static class MvcNewtonsoftOptionsExtensions
{
    public static MvcNewtonsoftJsonOptions ApplyGatewaySerializationConfiguration(this MvcNewtonsoftJsonOptions options)
    {
        ConfigurationBuilder
            .Build(new PresentationSerializationConfiguration())
            .ApplyToSerializationSettings(options.SerializerSettings);

        return options;
    }
}