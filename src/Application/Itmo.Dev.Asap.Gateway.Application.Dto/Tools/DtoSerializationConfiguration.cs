using FluentSerialization;
using Itmo.Dev.Asap.Gateway.Application.Dto.SubjectCourseAssociations;

namespace Itmo.Dev.Asap.Gateway.Application.Dto.Tools;

public class DtoSerializationConfiguration : ISerializationConfiguration
{
    public void Configure(ISerializationConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Type<GithubSubjectCourseAssociationDto>()
            .HasTypeKey("GithubSubjectCourseAssociation");

        configurationBuilder
            .Type<GoogleSubjectCourseAssociationDto>()
            .HasTypeKey("GoogleSubjectCourseAssociation");
    }
}