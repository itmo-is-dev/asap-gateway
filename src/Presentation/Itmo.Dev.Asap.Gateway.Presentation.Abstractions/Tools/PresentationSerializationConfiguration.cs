using FluentSerialization;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourses;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Tools;

public class PresentationSerializationConfiguration : ISerializationConfiguration
{
    public void Configure(ISerializationConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Type<CreateSubjectCourseGithubArgs>()
            .HasTypeKey(nameof(CreateSubjectCourseGithubArgs));
    }
}