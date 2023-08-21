using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Itmo.Dev.Asap.Core.Queue;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;
using Itmo.Dev.Asap.Gateway.Presentation.SignalR.Mapping;
using Microsoft.AspNetCore.SignalR;

namespace Itmo.Dev.Asap.Gateway.Presentation.SignalR.Queue;

public class QueueBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<QueueBackgroundService> _logger;

    public QueueBackgroundService(
        IServiceScopeFactory scopeFactory,
        ILogger<QueueBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested is false)
        {
            try
            {
                await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
                await ExecuteSingleAsync(scope.ServiceProvider, stoppingToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while listening to queue updates");
            }
        }
    }

#pragma warning disable CA1506
    private static async Task ExecuteSingleAsync(IServiceProvider provider, CancellationToken cancellationToken)
    {
        QueueService.QueueServiceClient client = provider.GetRequiredService<QueueService.QueueServiceClient>();

        IHubContext<QueueHub, IQueueHubClient> hubContext = provider
            .GetRequiredService<IHubContext<QueueHub, IQueueHubClient>>();

        IEnrichmentProcessor<string, StudentDtoBuilder, StudentDto> studentEnricher = provider
            .GetRequiredService<IEnrichmentProcessor<string, StudentDtoBuilder, StudentDto>>();

        AsyncServerStreamingCall<QueueUpdatedResponse>? stream = client
            .QueueUpdates(new Empty(), cancellationToken: cancellationToken);

        await foreach (QueueUpdatedResponse response in stream.ResponseStream.ReadAllAsync(cancellationToken))
        {
            IEnumerable<StudentDtoBuilder> studentBuilders = response.SubmissionsQueue.Students.Values
                .Select(x => x.MapToBuilder());

            IEnumerable<StudentDto> studentDtos = await studentEnricher.EnrichAsync(studentBuilders, cancellationToken);

            var studentMessages = studentDtos
                .Select(x => x.MapToMessage())
                .ToDictionary(x => x.User.Id);

            SubmissionMessage[] submissionMessages = response.SubmissionsQueue.Submissions
                .Select(x => x.MapToMessage())
                .ToArray();

            var message = new QueueUpdatedMessage(
                Guid.Parse(response.SubjectCourseId),
                Guid.Parse(response.StudentGroupId),
                response.StudentGroupName,
                new SubmissionQueueMessage(studentMessages, submissionMessages));

            string groupName = QueueHub.CombineIdentifiers(message.SubjectCourseId, message.StudentGroupId);

            await hubContext.Clients.Group(groupName).SendUpdateQueueMessage(message);
        }
    }
}