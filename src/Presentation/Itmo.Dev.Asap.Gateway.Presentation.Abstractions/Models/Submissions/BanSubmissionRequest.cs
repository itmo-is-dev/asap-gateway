namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Submissions;

public record BanSubmissionRequest(Guid StudentId, Guid AssignmentId);