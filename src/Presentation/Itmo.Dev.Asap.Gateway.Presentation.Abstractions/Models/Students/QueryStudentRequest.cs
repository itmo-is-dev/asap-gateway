namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students;

public record QueryStudentRequest(
    string? PageToken,
    int PageSize,
    IEnumerable<string> NamePatterns,
    IEnumerable<string> GroupNamePatterns,
    IEnumerable<int> UniversityIds,
    IEnumerable<string> GithubUsernamePatterns);