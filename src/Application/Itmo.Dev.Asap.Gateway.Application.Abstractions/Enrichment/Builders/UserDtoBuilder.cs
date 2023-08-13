using Itmo.Dev.Asap.Gateway.Application.Dto.Users;

namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;

public class UserDtoBuilder : IEntityBuilder<string, UserDto>
{
    public UserDtoBuilder(string id, string firstName, string middleName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }

    public string Id { get; }

    public string FirstName { get; }

    public string MiddleName { get; }

    public string LastName { get; }

    public string? GithubUsername { get; set; }

    public UserDto Build()
    {
        return new UserDto(
            Guid.Parse(Id),
            FirstName,
            MiddleName,
            LastName,
            GithubUsername);
    }
}