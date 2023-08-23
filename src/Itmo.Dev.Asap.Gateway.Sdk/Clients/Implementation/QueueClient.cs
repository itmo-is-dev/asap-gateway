using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;
using Itmo.Dev.Asap.Gateway.Sdk.Authentication;
using Itmo.Dev.Asap.Gateway.Sdk.Tools;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using Phazor.Extensions;
using System.Reactive.Subjects;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients.Implementation;

internal class QueueClient : IQueueClient, IAsyncDisposable
{
    private readonly Subject<QueueUpdatedMessage> _messageSubject;
    private readonly Subject<string> _errorSubject;
    private readonly HubConnection _connection;

    private readonly IDisposable _disposable;

    private readonly HashSet<(Guid SubjectCourseId, Guid StudentGroupId)> _subscriptions;

    public QueueClient(IOptions<GatewayOptions> options, ITokenProvider tokenProvider)
    {
        _messageSubject = new Subject<QueueUpdatedMessage>();
        _errorSubject = new Subject<string>();

        _subscriptions = new HashSet<(Guid SubjectCourseId, Guid StudentGroupId)>();

        _connection = new HubConnectionBuilder()
            .WithUrl(
                new Uri(options.Value.Uri, "hubs/queue"),
                o =>
                {
                    o.AccessTokenProvider = async () =>
                    {
                        string? token = await tokenProvider.FindTokenAsync(default);
                        return token is null ? null : $"Bearer {token}";
                    };
                })
            .Build();

        _disposable = _connection.On(
            "SendUpdateQueueMessage",
            (QueueUpdatedMessage m) => _messageSubject.OnNext(m));

        _disposable = _connection
            .On("SendError", (string error) => _errorSubject.OnNext(error))
            .Combine(_disposable);
    }

    public IObservable<QueueUpdatedMessage> QueueUpdates => _messageSubject;

    public IObservable<string> Errors => _errorSubject;

    public async Task SubscribeToQueueUpdates(
        Guid subjectCourseId,
        Guid studentGroupId,
        CancellationToken cancellationToken)
    {
        if (_connection.State is HubConnectionState.Disconnected)
            await _connection.StartAsync(cancellationToken);

        _subscriptions.Add((subjectCourseId, studentGroupId));

        await _connection.InvokeAsync(
            "SubscribeToQueueUpdates",
            subjectCourseId,
            studentGroupId,
            cancellationToken: cancellationToken);
    }

    public async Task UnsubscribeFromQueueUpdates(
        Guid subjectCourseId,
        Guid studentGroupId,
        CancellationToken cancellationToken)
    {
        if (_connection.State is HubConnectionState.Disconnected)
            return;

        _subscriptions.Remove((subjectCourseId, studentGroupId));

        await _connection.InvokeAsync(
            "UnsubscribeFromQueueUpdates",
            subjectCourseId,
            studentGroupId,
            cancellationToken: cancellationToken);

        if (_subscriptions.Count is 0)
            await _connection.StopAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        _messageSubject.Dispose();
        _errorSubject.Dispose();
        _disposable.Dispose();
    }
}