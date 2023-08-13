using Itmo.Dev.Asap.Gateway.Application.Dto.Users;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students;

public record QueryStudentResponse(string? PageToken, IEnumerable<StudentDto> Students);