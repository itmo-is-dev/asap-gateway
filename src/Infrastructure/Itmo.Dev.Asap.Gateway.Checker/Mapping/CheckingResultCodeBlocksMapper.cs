using Itmo.Dev.Asap.Checker.Models;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

namespace Itmo.Dev.Asap.Gateway.Checker.Mapping;

public static class CheckingResultCodeBlocksMapper
{
    public static SimilarCodeBlocksDto MapToDto(this SimilarCodeBlocks codeBlocks)
    {
        return new SimilarCodeBlocksDto(
            codeBlocks.First.MapToDto(),
            codeBlocks.Second.MapToDto(),
            codeBlocks.SimilarityScore);
    }

    public static CodeBlockDto MapToDto(this CodeBlock codeBlock)
    {
        return new CodeBlockDto(
            codeBlock.FilePath,
            codeBlock.LineFrom,
            codeBlock.LineTo,
            codeBlock.Content);
    }
}