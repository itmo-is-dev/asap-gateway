using Itmo.Dev.Asap.Gateway.Application.Dto.Users;

namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;

public class StudentDtoBuilder : IEntityBuilder<string, StudentDto>
{
    public StudentDtoBuilder(UserDtoBuilder user, Guid? groupId, string groupName)
    {
        Id = user.Id;
        User = user;
        GroupId = groupId;
        GroupName = groupName;
    }

    public string Id { get; }

    public UserDtoBuilder User { get; }

    public Guid? GroupId { get; }

    public string GroupName { get; }

    public StudentDto Build()
    {
        return new StudentDto(User.Build(), GroupId, GroupName);
    }
}