using Itmo.Dev.Asap.Core.Permissions;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Itmo.Dev.Asap.Gateway.Presentation.SignalR.Queue;

public class QueueHub : Hub<IQueueHubClient>
{
    private const string Scope = "Queue";

    private readonly ILogger<QueueHub> _logger;

    public QueueHub(ILogger<QueueHub> logger)
    {
        _logger = logger;
    }

    public static string CombineIdentifiers(Guid courseId, Guid groupId)
    {
        return string.Concat(courseId, groupId);
    }

    [AuthorizeFeature(Scope, nameof(SubscribeToQueueUpdates))]
    public async Task SubscribeToQueueUpdates(Guid subjectCourseId, Guid studentGroupId)
    {
        IServiceProvider? services = Context.GetHttpContext()?.RequestServices;

        if (services is null)
            return;

        await ExecuteSafeAsync(async () =>
        {
            PermissionService.PermissionServiceClient client = services
                .GetRequiredService<PermissionService.PermissionServiceClient>();

            var request = new HasAccessToSubjectCourseRequest { SubjectCourseId = subjectCourseId.ToString() };

            HasAccessToSubjectCourseResponse response = await client
                .HasAccessToSubjectCourseAsync(request, cancellationToken: Context.ConnectionAborted);

            if (response.HasAccess)
            {
                string groupName = CombineIdentifiers(subjectCourseId, studentGroupId);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            }
        });
    }

    [AuthorizeFeature(Scope, nameof(UnsubscribeFromQueueUpdates))]
    public Task UnsubscribeFromQueueUpdates(Guid courseId, Guid groupId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, CombineIdentifiers(courseId, groupId));
    }

    private async Task ExecuteSafeAsync(Func<Task> func)
    {
        try
        {
            await func.Invoke();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to execute hub request");
            await Clients.Caller.SendError("Failed to process request");
        }
    }
}