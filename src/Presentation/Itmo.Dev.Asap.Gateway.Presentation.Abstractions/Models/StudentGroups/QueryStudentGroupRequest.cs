namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.StudentGroups;

public record QueryStudentGroupRequest(
    string? PageToken,
    int PageSize,
    IEnumerable<Guid> ExcludedIds,
    IEnumerable<string> NamePatterns,
    IEnumerable<Guid> ExcludedSubjectCourseIds);