namespace Itmo.Dev.Asap.Gateway.Application.Dto.Querying;

public record QueryConfiguration<T>(IReadOnlyCollection<QueryParameter<T>> Parameters);