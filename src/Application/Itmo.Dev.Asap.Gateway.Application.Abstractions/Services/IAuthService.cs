namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Services;

public interface IAuthService
{
    Task<bool> IsTokenValid(string token, CancellationToken cancellationToken);
}