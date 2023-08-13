namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students;

public record CreateStudentRequest(string? FirstName, string? MiddleName, string? LastName, Guid GroupId);