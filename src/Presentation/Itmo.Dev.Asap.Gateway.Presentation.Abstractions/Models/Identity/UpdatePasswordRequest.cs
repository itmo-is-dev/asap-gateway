namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity;

public record UpdatePasswordRequest(string CurrentPassword, string NewPassword);