namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourseGroups;

public record QuerySubjectCourseGroupsRequest(Guid SubjectCourseId, IEnumerable<Guid> Ids, IEnumerable<string> Names);