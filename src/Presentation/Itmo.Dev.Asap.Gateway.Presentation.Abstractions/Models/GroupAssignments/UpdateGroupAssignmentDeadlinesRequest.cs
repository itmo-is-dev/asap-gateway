namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.GroupAssignments;

public record UpdateGroupAssignmentDeadlinesRequest(DateTimeOffset Deadline, IEnumerable<Guid> GroupIds);