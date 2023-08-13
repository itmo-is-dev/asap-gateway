using Itmo.Dev.Asap.Gateway.Application.Dto.Tables;

namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Queue;

public record SubjectCourseQueueModel(Guid SubjectCourseId, Guid StudentGroupId, SubmissionsQueueDto Queue);