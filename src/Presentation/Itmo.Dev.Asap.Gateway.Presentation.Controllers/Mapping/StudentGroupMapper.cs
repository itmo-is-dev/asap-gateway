using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.StudentGroups;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;

[Mapper]
public static partial class StudentGroupMapper
{
    public static partial Asap.Core.StudentGroups.QueryStudentGroupRequest ToProto(this QueryStudentGroupRequest request);
}