using Google.Protobuf.WellKnownTypes;
using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Core.Mapping;

[Mapper(EnumMappingStrategy = EnumMappingStrategy.ByName)]
public static partial class ProtoMapper
{
    public static partial AssignmentDto ToDto(this Assignment assignment);

    public static partial GroupAssignmentDto ToDto(this GroupAssignment groupAssignment);

    public static partial StudentGroupDto ToDto(this StudentGroup studentGroup);

    public static partial SubjectDto ToDto(this Subject subject);

    public static partial SubjectCourseGroupDto ToDto(this SubjectCourseGroup subjectCourseGroup);

    public static partial SubmissionDto ToDto(this Submission submission);

    public static partial SubmissionStateWorkflowTypeDto ToDto(this SubmissionStateWorkflowType workflowType);

    public static partial SubmissionStateWorkflowType ToProto(this SubmissionStateWorkflowTypeDto workflowType);

    private static partial Guid ToGuid(this string value);

    private static DateTime ToDateTime(Timestamp timestamp)
    {
        return timestamp.ToDateTime();
    }
}