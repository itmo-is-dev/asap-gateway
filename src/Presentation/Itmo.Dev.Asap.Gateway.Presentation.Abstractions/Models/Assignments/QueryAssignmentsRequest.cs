namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Assignments;

public record QueryAssignmentsRequest(
    IEnumerable<Guid> Ids,
    IEnumerable<string> Names,
    IEnumerable<Guid> SubjectCourseIds);