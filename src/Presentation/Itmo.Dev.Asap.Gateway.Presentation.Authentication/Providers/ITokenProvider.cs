namespace Itmo.Dev.Asap.Gateway.Presentation.Authentication.Providers;

public interface ITokenProvider
{
    string? Token { get; }
}