using Itmo.Dev.Asap.Auth;
using Itmo.Dev.Asap.Gateway.Application.Dto.Identity;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;

[Mapper]
public static partial class IdentityMapper
{
    [MapProperty(
        nameof(Abstractions.Models.Identity.CreateUserAccountRequest.RoleName),
        nameof(CreateUserAccountRequest.Role))]
    public static partial CreateUserAccountRequest ToProto(
        this Abstractions.Models.Identity.CreateUserAccountRequest request);

    public static partial PasswordOptionsDto ToDto(this GetPasswordOptionsResponse response);
}