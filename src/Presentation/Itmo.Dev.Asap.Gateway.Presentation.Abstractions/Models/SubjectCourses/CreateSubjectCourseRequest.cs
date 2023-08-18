using Itmo.Dev.Asap.Gateway.Application.Dto.Study;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourses;

public record CreateSubjectCourseRequest(
    Guid SubjectId,
    string Name,
    SubmissionStateWorkflowTypeDto WorkflowType,
    CreateSubjectCourseArgs? Args);

public abstract record CreateSubjectCourseArgs;

public sealed record CreateSubjectCourseGithubArgs(
    string OrganizationName,
    string TemplateRepositoryName,
    string MentorTeamName) : CreateSubjectCourseArgs;