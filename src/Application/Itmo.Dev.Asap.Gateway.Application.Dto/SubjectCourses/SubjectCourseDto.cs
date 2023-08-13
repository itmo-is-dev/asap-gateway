using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourseAssociations;

namespace Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;

public record SubjectCourseDto(
    Guid Id,
    Guid SubjectId,
    string Title,
    SubmissionStateWorkflowTypeDto? WorkflowType,
    IReadOnlyCollection<SubjectCourseAssociationDto> Associations);