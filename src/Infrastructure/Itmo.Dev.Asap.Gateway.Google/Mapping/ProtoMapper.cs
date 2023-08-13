using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourseAssociations;
using Itmo.Dev.Asap.Google;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Google.Mapping;

[Mapper]
public static partial class ProtoMapper
{
    [MapProperty(nameof(GoogleSubjectCourse.Id), nameof(GoogleSubjectCourseAssociationDto.SubjectCourseId))]
    public static partial GoogleSubjectCourseAssociationDto MapToAssociation(this GoogleSubjectCourse subjectCourse);
}