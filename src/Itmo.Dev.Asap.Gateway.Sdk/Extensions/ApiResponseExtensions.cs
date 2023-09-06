using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Extensions;

public static class ApiResponseExtensions
{
    public static Task<ErrorDetails?> TryGetErrorDetailsAsync(this IApiResponse response)
    {
        return response.Error is null
            ? Task.FromResult<ErrorDetails?>(null)
            : response.Error.GetContentAsAsync<ErrorDetails>();
    }
}