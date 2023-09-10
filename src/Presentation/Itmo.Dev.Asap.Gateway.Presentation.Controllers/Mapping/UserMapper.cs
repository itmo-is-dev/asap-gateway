using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Users;
using Itmo.Dev.Asap.Github.Users;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;

[Mapper]
public static partial class UserMapper
{
    public static partial UpdateUsernameRequest MapToProtoRequest(this UpdateGithubUsernameRequest request);

    public static partial UserDtoBuilder ToBuilder(this User user);
}