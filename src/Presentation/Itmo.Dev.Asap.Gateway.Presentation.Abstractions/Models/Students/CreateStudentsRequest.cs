namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Students;

public record CreateStudentsRequest(IEnumerable<CreateStudentsRequest.Student> Students)
{
    public sealed record Student(
        string FirstName,
        string MiddleName,
        string LastName,
        int UniversityId,
        Guid GroupId);
}