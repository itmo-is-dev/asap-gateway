namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;

public record GetCheckingsRequest(Guid SubjectCourseId, int PageSize, string? PageToken);