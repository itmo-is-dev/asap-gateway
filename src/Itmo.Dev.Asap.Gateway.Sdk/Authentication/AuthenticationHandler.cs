using System.Net.Http.Headers;

namespace Itmo.Dev.Asap.Gateway.Sdk.Authentication;

internal class AuthenticationHandler : DelegatingHandler
{
    private readonly ITokenProvider _tokenProvider;

    public AuthenticationHandler(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return SendAsync(request, cancellationToken).GetAwaiter().GetResult();
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string? token = await _tokenProvider.FindTokenAsync(cancellationToken);

        if (string.IsNullOrEmpty(token) is false)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}