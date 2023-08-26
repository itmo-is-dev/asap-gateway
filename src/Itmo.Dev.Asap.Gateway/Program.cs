#pragma warning disable CA1506

using FluentSerialization.Extensions.NewtonsoftJson;
using Itmo.Dev.Asap.Gateway.Application.Dto.Tools;
using Itmo.Dev.Asap.Gateway.Application.Extensions;
using Itmo.Dev.Asap.Gateway.Auth.Extensions;
using Itmo.Dev.Asap.Gateway.Core.Extensions;
using Itmo.Dev.Asap.Gateway.Extensions;
using Itmo.Dev.Asap.Gateway.Github.Extensions;
using Itmo.Dev.Asap.Gateway.Google.Extensions;
using Itmo.Dev.Asap.Gateway.Grpc.Extensions;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Tools;
using Itmo.Dev.Asap.Gateway.Presentation.Authentication.Extensions;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Extensions;
using Itmo.Dev.Asap.Gateway.Presentation.SignalR.Extensions;
using Itmo.Dev.Platform.Logging.Extensions;
using Itmo.Dev.Platform.YandexCloud.Extensions;
using SerializationConfigurationBuilder = FluentSerialization.ConfigurationBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("features.json");

await builder.AddYandexCloudConfigurationAsync();

builder.Services
    .AddApplication()
    .AddAuthGrpcClients()
    .AddCoreGrpcClients()
    .AddGithubGrpcClients()
    .AddGoogleGrpcClients()
    .AddGrpcInfrastructure(builder.Configuration)
    .AddGatewayAuthentication()
    .AddGatewayAuthorization();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => SerializationConfigurationBuilder
        .Build(new PresentationSerializationConfiguration(), new DtoSerializationConfiguration())
        .ApplyToSerializationSettings(options.SerializerSettings))
    .AddPresentationControllers();

builder.Services.AddSignalrPresentation();

builder.Services.AddCors(o => o.AddDefaultPolicy(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddSwaggerConfiguration();

builder.AddPlatformSentry();
builder.Host.AddPlatformSerilog(builder.Configuration);

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UsePlatformSentryTracing(builder.Configuration);
app.UseCors();
app.UseAuthorization();

app.MapControllers();
app.UseSignalrPresentation();

await app.RunAsync();