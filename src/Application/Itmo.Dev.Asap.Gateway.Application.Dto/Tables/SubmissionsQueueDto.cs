using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;

namespace Itmo.Dev.Asap.Gateway.Application.Dto.Tables;

public record struct SubmissionsQueueDto(
    string GroupName,
    IReadOnlyCollection<StudentDto> Students,
    IReadOnlyList<SubmissionDto> Submissions);