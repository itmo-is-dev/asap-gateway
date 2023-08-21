using Itmo.Dev.Asap.Core.Queue;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Presentation.SignalR.Mapping;

[Mapper]
internal static partial class QueueHubMapper
{
    public static partial StudentMessage MapToMessage(this StudentDto student);

    public static partial SubmissionMessage MapToMessage(this QueueUpdatedResponse.Types.Submission submission);
}