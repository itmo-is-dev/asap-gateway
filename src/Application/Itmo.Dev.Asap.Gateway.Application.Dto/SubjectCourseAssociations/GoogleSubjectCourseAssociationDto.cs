namespace Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourseAssociations;

public record GoogleSubjectCourseAssociationDto(
    Guid SubjectCourseId,
    string SpreadsheetId) : SubjectCourseAssociationDto(SubjectCourseId);