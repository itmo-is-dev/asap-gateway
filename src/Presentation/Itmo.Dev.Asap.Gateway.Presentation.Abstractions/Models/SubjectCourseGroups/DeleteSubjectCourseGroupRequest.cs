namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourseGroups;

public record DeleteSubjectCourseGroupRequest(Guid SubjectCourseId, Guid GroupId);