namespace Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

public record CheckingDto(long Id, DateTimeOffset CreatedAt, bool IsCompleted);