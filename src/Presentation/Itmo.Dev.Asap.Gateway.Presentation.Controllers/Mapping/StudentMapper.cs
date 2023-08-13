using Itmo.Dev.Asap.Core.Models;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;

[Mapper]
public static partial class StudentMapper
{
    public static partial Asap.Core.Students.CreateStudentRequest ToProto(this CreateStudentRequest request);

    public static partial Asap.Core.Students.QueryStudentRequest ToProto(this QueryStudentRequest request);

    [MapProperty(nameof(Student.User), "user")]
    public static partial StudentDtoBuilder MapToBuilder(this Student student);
}