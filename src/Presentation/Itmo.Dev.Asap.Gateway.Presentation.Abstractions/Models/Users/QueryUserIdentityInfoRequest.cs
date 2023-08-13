namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Users;

public record QueryUserIdentityInfoRequest(
    string? PageToken,
    int PageSize,
    IEnumerable<string> NamePatterns,
    IEnumerable<int> UniversityIds);