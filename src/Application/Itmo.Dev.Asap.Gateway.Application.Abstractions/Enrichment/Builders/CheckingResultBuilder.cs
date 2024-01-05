using Itmo.Dev.Asap.Gateway.Application.Dto.Checking;

namespace Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;

public class CheckingResultBuilder : IEntityBuilder<CheckingResultKey, CheckingResultDto>
{
    public CheckingResultBuilder(
        Guid assignmentId,
        CheckingResultSubmissionInfo firstSubmission,
        CheckingResultSubmissionInfo secondSubmission,
        double similarityScore)
    {
        AssignmentId = assignmentId;
        FirstSubmission = firstSubmission;
        SecondSubmission = secondSubmission;
        SimilarityScore = similarityScore;

        Id = new CheckingResultKey(firstSubmission.SubmissionId, secondSubmission.SubmissionId);
    }

    public CheckingResultKey Id { get; }

    public Guid AssignmentId { get; }

    public CheckingResultSubmissionInfo FirstSubmission { get; }

    public CheckingResultSubmissionInfo SecondSubmission { get; }

    public double SimilarityScore { get; }

    public string? AssignmentName { get; set; }

    public CheckingResultStudentInfo? FirstSubmissionStudent { get; set; }

    public CheckingResultStudentInfo? SecondSubmissionStudent { get; set; }

    public CheckingResultDto Build()
    {
        var first = new CheckingResultSubmissionDto(
            SubmissionId: FirstSubmission.SubmissionId,
            StudentId: FirstSubmission.UserId,
            FirstName: FirstSubmissionStudent?.FirstName ?? string.Empty,
            LastName: FirstSubmissionStudent?.LastName ?? string.Empty,
            GroupName: FirstSubmissionStudent?.GroupName ?? string.Empty);

        var second = new CheckingResultSubmissionDto(
            SubmissionId: SecondSubmission.SubmissionId,
            StudentId: SecondSubmission.UserId,
            FirstName: SecondSubmissionStudent?.FirstName ?? string.Empty,
            LastName: SecondSubmissionStudent?.LastName ?? string.Empty,
            GroupName: SecondSubmissionStudent?.GroupName ?? string.Empty);

        return new CheckingResultDto(
            AssignmentName ?? string.Empty,
            first,
            second,
            SimilarityScore);
    }
}