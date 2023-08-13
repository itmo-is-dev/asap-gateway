using Itmo.Dev.Asap.Gateway.Application.Dto.Study;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.StudentGroups;

public record QueryStudentGroupResponse(string? PageToken, IEnumerable<StudentGroupDto> StudentGroups);