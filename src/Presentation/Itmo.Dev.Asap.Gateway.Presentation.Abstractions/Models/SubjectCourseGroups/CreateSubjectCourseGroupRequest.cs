namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourseGroups;

public record CreateSubjectCourseGroupRequest(Guid SubjectCourseId, Guid GroupId);