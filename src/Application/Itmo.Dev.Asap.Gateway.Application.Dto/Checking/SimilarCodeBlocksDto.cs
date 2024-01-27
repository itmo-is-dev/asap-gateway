namespace Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

public record SimilarCodeBlocksDto(CodeBlockDto First, CodeBlockDto Second, double SimilarityScore);