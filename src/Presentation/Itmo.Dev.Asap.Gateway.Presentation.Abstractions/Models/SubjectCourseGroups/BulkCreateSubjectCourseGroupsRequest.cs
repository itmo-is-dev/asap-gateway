namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourseGroups;

public record BulkCreateSubjectCourseGroupsRequest(Guid SubjectCourseId, IReadOnlyCollection<Guid> GroupIds);