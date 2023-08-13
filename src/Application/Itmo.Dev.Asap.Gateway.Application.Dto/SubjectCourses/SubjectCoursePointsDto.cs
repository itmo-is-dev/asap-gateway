using Itmo.Dev.Asap.Gateway.Application.Dto.Study;
using Itmo.Dev.Asap.Gateway.Application.Dto.Tables;

namespace Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourses;

public record struct SubjectCoursePointsDto(
    IReadOnlyCollection<AssignmentDto> Assignments,
    IReadOnlyList<StudentPointsDto> StudentsPoints);