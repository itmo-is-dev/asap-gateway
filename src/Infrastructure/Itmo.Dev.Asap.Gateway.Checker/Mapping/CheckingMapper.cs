using Itmo.Dev.Asap.Checker.Models;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

namespace Itmo.Dev.Asap.Gateway.Checker.Mapping;

public static class CheckingMapper
{
    public static CheckingDto MapToDto(this CheckingTask task)
    {
        return new CheckingDto(
            task.TaskId,
            task.CreatedAt.ToDateTimeOffset(),
            task.IsCompleted);
    }
}