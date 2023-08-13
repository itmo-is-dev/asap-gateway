namespace Itmo.Dev.Asap.Gateway.Sdk.Authentication;

public interface ITokenProvider
{
    ValueTask<string?> FindTokenAsync(CancellationToken cancellationToken);
}