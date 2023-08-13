namespace Itmo.Dev.Asap.Gateway.Application.Dto.Study;

public record GroupAssignmentDto(
    Guid GroupId,
    string GroupName,
    Guid AssignmentId,
    string AssignmentTitle,
    DateTime Deadline);