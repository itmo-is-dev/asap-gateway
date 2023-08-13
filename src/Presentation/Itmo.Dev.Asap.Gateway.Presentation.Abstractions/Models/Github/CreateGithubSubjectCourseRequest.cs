using Itmo.Dev.Asap.Gateway.Application.Dto.Study;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Github;

public record CreateGithubSubjectCourseRequest(
    Guid SubjectId,
    string Name,
    SubmissionStateWorkflowTypeDto WorkflowType,
    string GithubOrganizationName,
    string TemplateRepositoryName,
    string MentorTeamName);