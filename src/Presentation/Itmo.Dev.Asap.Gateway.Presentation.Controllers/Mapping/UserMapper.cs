using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;

[Mapper]
public static partial class UserMapper
{
    public static partial UserDtoBuilder ToBuilder(this User user);
}