namespace Itmo.Dev.Asap.Gateway.Application.Dto.Users;

public record StudentDto(UserDto User, Guid? GroupId, string GroupName, int? UniversityId);