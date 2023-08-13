using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students;
using Refit;

namespace Itmo.Dev.Asap.Gateway.Sdk.Clients;

public interface IStudentClient
{
    [Post("/api/student")]
    Task<IApiResponse<StudentDto>> CreateAsync(
        [Body] CreateStudentRequest request,
        CancellationToken cancellationToken);

    [Put("/api/student/{studentId}/dismiss")]
    Task<IApiResponse> DismissFromGroupAsync(Guid studentId, CancellationToken cancellationToken);

    [Put("/api/student/{studentId}/group")]
    Task<IApiResponse<StudentDto>> TransferToGroupAsync(
        Guid studentId,
        [Body] TransferStudentRequest request,
        CancellationToken cancellationToken);

    [Post("/api/student/query")]
    Task<IApiResponse<QueryStudentResponse>> QueryAsync(
        [Body] QueryStudentRequest request,
        CancellationToken cancellationToken);
}