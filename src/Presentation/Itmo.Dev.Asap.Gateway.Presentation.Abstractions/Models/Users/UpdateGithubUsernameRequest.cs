namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Users;

public record UpdateGithubUsernameRequest(IEnumerable<UpdateGithubUsernameRequest.Model> Models)
{
    public sealed record Model(Guid UserId, string GithubUsername);
}