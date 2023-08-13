namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.SubjectCourses;

public record AddFractionPolicyRequest(TimeSpan SpanBeforeActivation, double Fraction);