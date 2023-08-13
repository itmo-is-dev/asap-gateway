namespace Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourseAssociations;

public record GithubSubjectCourseAssociationDto(
    Guid SubjectCourseId,
    string OrganizationName,
    string TemplateRepositoryName,
    string MentorTeamName) : SubjectCourseAssociationDto(SubjectCourseId);