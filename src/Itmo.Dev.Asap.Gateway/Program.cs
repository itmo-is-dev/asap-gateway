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
using SerializationConfigurationBuilder = FluentSerialization.ConfigurationBuilder;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("features.json");

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

builder.Services.AddCors(o => o.AddDefaultPolicy(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
builder.Services.AddSwaggerConfiguration();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();