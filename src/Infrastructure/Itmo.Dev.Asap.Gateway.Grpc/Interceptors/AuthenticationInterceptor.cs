using Grpc.Core;
using Grpc.Core.Interceptors;
using Itmo.Dev.Asap.Gateway.Presentation.Authentication.Providers;

namespace Itmo.Dev.Asap.Gateway.Grpc.Interceptors;

public class AuthenticationInterceptor : Interceptor
{
    private readonly ITokenProvider _tokenProvider;

    public AuthenticationInterceptor(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
        TRequest request,
        ClientInterceptorContext<TRequest, TResponse> context,
        AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        string? token = _tokenProvider.Token;

        if (string.IsNullOrEmpty(token))
            return continuation.Invoke(request, context);

        var headers = new Metadata
        {
            new Metadata.Entry("authorization", token),
        };

        if (context.Options.Headers is not null)
        {
            foreach (Metadata.Entry entry in context.Options.Headers)
            {
                headers.Add(entry);
            }
        }

        CallOptions options = context.Options.WithHeaders(headers);
        context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);

        return continuation.Invoke(request, context);
    }
}