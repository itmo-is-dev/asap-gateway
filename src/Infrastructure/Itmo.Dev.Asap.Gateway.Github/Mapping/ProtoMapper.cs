using Itmo.Dev.Asap.Gateway.Application.Dto.Github;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourseAssociations;
using Itmo.Dev.Asap.Github.Models;
using Riok.Mapperly.Abstractions;

namespace Itmo.Dev.Asap.Gateway.Github.Mapping;

[Mapper]
public static partial class ProtoMapper
{
    [MapProperty(nameof(GithubSubjectCourse.Id), nameof(GithubSubjectCourseAssociationDto.SubjectCourseId))]
    public static partial GithubSubjectCourseAssociationDto MapToAssociation(this GithubSubjectCourse subjectCourse);

    public static partial GithubOrganizationDto MapToDto(this GithubOrganization organization);

    public static partial GithubRepositoryDto MapToDto(this GithubRepository repository);

    public static partial GithubTeamDto MapToDto(this GithubTeam team);
}