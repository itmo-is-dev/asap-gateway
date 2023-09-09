using Itmo.Dev.Asap.Auth;
using Itmo.Dev.Asap.Core.Users;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Identity;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Extensions;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CreateUserAccountRequest =
    Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity.CreateUserAccountRequest;
using LoginRequest = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity.LoginRequest;
using LoginResponse = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity.LoginResponse;
using UpdatePasswordRequest = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity.UpdatePasswordRequest;
using UpdatePasswordResponse = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity.UpdatePasswordResponse;
using UpdateUsernameRequest = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity.UpdateUsernameRequest;
using UpdateUsernameResponse = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Identity.UpdateUsernameResponse;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    private const string Scope = "Identity";

    private readonly IdentityService.IdentityServiceClient _identityClient;
    private readonly UserService.UserServiceClient _userClient;
    private readonly IEnrichmentProcessor<string, UserDtoBuilder, UserDto> _enrichmentProcessor;

    public IdentityController(
        IdentityService.IdentityServiceClient identityClient,
        UserService.UserServiceClient userClient,
        IEnrichmentProcessor<string, UserDtoBuilder, UserDto> enrichmentProcessor)
    {
        _identityClient = identityClient;
        _userClient = userClient;
        _enrichmentProcessor = enrichmentProcessor;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> LoginAsync(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new Asap.Auth.LoginRequest
        {
            Username = request.Username,
            Password = request.Password,
        };

        Asap.Auth.LoginResponse grpcResponse = await _identityClient
            .LoginAsync(grpcRequest, cancellationToken: cancellationToken);

        var response = new LoginResponse(grpcResponse.Token);

        return Ok(response);
    }

    [HttpPut("users/{username}/role")]
    [AuthorizeFeature(Scope, nameof(ChangeUserRole))]
    public async Task<IActionResult> ChangeUserRole(
        string username,
        [FromQuery] string roleName,
        CancellationToken cancellationToken)
    {
        var request = new ChangeUserRoleRequest
        {
            Username = username,
            Role = roleName,
        };

        await _identityClient.ChangeUserRoleAsync(request, cancellationToken: cancellationToken);

        return Ok();
    }

    [HttpPost("user/{id:guid}/account")]
    [AuthorizeFeature(Scope, nameof(CreateUserAccount))]
    public async Task<ActionResult<UserIdentityInfoDto>> CreateUserAccount(
        Guid id,
        [FromBody] CreateUserAccountRequest request,
        CancellationToken cancellationToken)
    {
        Asap.Auth.CreateUserAccountRequest grpcRequest = request.ToProto();
        grpcRequest.UserId = id.ToString();

        await _identityClient.CreateUserAccountAsync(grpcRequest, cancellationToken: cancellationToken);

        var userRequest = new FindByIdRequest { UserId = grpcRequest.UserId };

        FindByIdResponse? userResponse = await _userClient
            .FindByIdAsync(userRequest, cancellationToken: cancellationToken);

        if (userResponse.UserCase is not FindByIdResponse.UserOneofCase.UserValue)
            return NotFound();

        UserDtoBuilder[] builders = { userResponse.UserValue.ToBuilder() };
        IEnumerable<UserDto> users = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        var user = new UserIdentityInfoDto(users.Single(), true);

        return Ok(user);
    }

    [Authorize]
    [HttpPut("username")]
    public async Task<ActionResult<UpdateUsernameResponse>> UpdateUsername(
        [FromBody] UpdateUsernameRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new Asap.Auth.UpdateUsernameRequest
        {
            Username = request.Username,
            IssuerId = HttpContext.User.GetId().ToString(),
        };

        Asap.Auth.UpdateUsernameResponse grpcResponse = await _identityClient
            .UpdateUsernameAsync(grpcRequest, cancellationToken: cancellationToken);

        var response = new UpdateUsernameResponse(grpcResponse.Token);

        return Ok(response);
    }

    [Authorize]
    [HttpPut("password")]
    public async Task<ActionResult<UpdatePasswordResponse>> UpdatePasswordAsync(
        [FromBody] UpdatePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new Asap.Auth.UpdatePasswordRequest
        {
            CurrentPassword = request.CurrentPassword,
            NewPassword = request.NewPassword,
        };

        Asap.Auth.UpdatePasswordResponse grpcResponse = await _identityClient
            .UpdatePasswordAsync(grpcRequest, cancellationToken: cancellationToken);

        var response = new UpdatePasswordResponse(grpcResponse.Token);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("password/options")]
    public async Task<PasswordOptionsDto> GetPasswordOptionsAsync(CancellationToken cancellationToken)
    {
        var request = new GetPasswordOptionsRequest();

        GetPasswordOptionsResponse response = await _identityClient
            .GetPasswordOptionsAsync(request, cancellationToken: cancellationToken);

        return response.ToDto();
    }

    [AuthorizeFeature(Scope, nameof(GetRoles))]
    [HttpGet("roles")]
    public async Task<ActionResult<IEnumerable<string>>> GetRoles(CancellationToken cancellationToken)
    {
        var request = new GetRoleNamesRequest();

        GetRoleNamesResponse response = await _identityClient
            .GetRoleNamesAsync(request, cancellationToken: cancellationToken);

        return Ok(response.RoleName);
    }
}