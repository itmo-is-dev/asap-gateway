namespace Itmo.Dev.Asap.Gateway.Application.Dto.Users;

public record UserDto(Guid Id, string FirstName, string MiddleName, string LastName, string? GithubUsername);