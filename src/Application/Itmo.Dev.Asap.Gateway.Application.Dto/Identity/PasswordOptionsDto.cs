namespace Itmo.Dev.Asap.Gateway.Application.Dto.Identity;

public record PasswordOptionsDto(
    bool RequireDigit,
    bool RequireLowercase,
    bool RequireNonAlphanumeric,
    bool RequireUppercase,
    int RequiredLength,
    int RequiredUniqueChars);