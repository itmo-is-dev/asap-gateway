namespace Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

public record CodeBlockDto(string FilePath, int LineFrom, int LineTo, string Content);