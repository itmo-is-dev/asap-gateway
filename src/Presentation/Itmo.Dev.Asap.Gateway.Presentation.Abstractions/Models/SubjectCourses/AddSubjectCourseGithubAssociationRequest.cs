namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourses;

public record AddSubjectCourseGithubAssociationRequest(
    string OrganizationName,
    string TemplateRepositoryName,
    string MentorTeamName);