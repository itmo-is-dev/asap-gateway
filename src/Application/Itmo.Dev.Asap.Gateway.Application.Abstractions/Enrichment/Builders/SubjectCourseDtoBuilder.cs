using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourseAssociations;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;

namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;

public class SubjectCourseDtoBuilder : IEntityBuilder<string, SubjectCourseDto>
{
    private readonly List<SubjectCourseAssociationDto> _associations;

    public SubjectCourseDtoBuilder(
        string id,
        Guid subjectId,
        string title,
        SubmissionStateWorkflowTypeDto? workflowType)
    {
        Id = id;
        SubjectCourseId = Guid.Parse(id);
        SubjectId = subjectId;
        Title = title;
        WorkflowType = workflowType;

        _associations = new List<SubjectCourseAssociationDto>();
    }

    public string Id { get; }

    public Guid SubjectCourseId { get; }

    public Guid SubjectId { get; }

    public string Title { get; }

    public SubmissionStateWorkflowTypeDto? WorkflowType { get; }

    public void AddAssociation(SubjectCourseAssociationDto association)
        => _associations.Add(association);

    public SubjectCourseDto Build()
    {
        return new SubjectCourseDto(
            SubjectCourseId,
            SubjectId,
            Title,
            WorkflowType,
            _associations);
    }
}