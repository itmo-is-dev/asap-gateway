namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;

public record CreateAssignmentRequest(
    Guid SubjectCourseId,
    string Title,
    string ShortName,
    int Order,
    double MinPoints,
    double MaxPoints);