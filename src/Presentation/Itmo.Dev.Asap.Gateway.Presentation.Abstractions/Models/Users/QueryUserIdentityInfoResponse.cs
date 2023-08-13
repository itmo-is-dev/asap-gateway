using Itmo.Dev.Asap.Gateway.Application.Dto.Users;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Users;

public record QueryUserIdentityInfoResponse(IEnumerable<UserIdentityInfoDto> Users, string? PageToken);