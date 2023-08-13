namespace Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity;

public record CreateUserAccountRequest(string Username, string Password, string RoleName);
