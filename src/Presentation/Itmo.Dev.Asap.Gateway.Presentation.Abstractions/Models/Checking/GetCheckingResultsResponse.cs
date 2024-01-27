using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Checking;

public record GetCheckingResultsResponse(IEnumerable<CheckingResultDto> Results, string? PageToken);