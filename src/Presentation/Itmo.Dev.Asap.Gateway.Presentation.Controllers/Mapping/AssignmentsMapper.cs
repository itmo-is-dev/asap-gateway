using Itmo.Dev.Asap.Core.Assignments;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;

[Mapper]
public static partial class AssignmentsMapper
{
    public static partial CreateRequest ToProto(this CreateAssignmentRequest request);
}