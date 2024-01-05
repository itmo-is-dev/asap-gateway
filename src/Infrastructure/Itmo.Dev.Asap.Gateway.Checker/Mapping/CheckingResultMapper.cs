using Itmo.Dev.Asap.Checker.Models;
using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

namespace Itmo.Dev.Asap.Gateway.Checker.Mapping;

public static class CheckingResultMapper
{
    public static CheckingResultSubmissionInfo MapToDto(this SubmissionInfo info)
    {
        return new CheckingResultSubmissionInfo(
            Guid.Parse(info.SubmissionId),
            Guid.Parse(info.UserId),
            Guid.Parse(info.GroupId));
    }
}