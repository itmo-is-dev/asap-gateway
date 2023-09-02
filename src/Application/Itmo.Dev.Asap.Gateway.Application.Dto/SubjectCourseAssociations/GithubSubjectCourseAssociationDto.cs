namespace Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourseAssociations;

public record GithubSubjectCourseAssociationDto(
    Guid SubjectCourseId,
    long OrganizationId,
    string OrganizationName,
    long TemplateRepositoryId,
    string TemplateRepositoryName,
    long MentorTeamId,
    string MentorTeamName) : SubjectCourseAssociationDto(SubjectCourseId);