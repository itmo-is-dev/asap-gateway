using Itmo.Dev.Asap.Auth;
using Itmo.Dev.Asap.Core.Users;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment;
using Itmo.Dev.Asap.Gateway.Application.Abstractions.Enrichment.Builders;
using Itmo.Dev.Asap.Gateway.Application.Dto.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Users;
using Itmo.Dev.Asap.Gateway.Presentation.Authorization;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Extensions;
using Itmo.Dev.Asap.Gateway.Presentation.Controllers.Mapping;
using Itmo.Dev.Asap.Github.Users;
using Microsoft.AspNetCore.Mvc;
using UpdateNameRequest = Itmo.Dev.Asap.Gateway.Presentation.Abstractions.Models.Users.UpdateNameRequest;
using UpdateUsernameRequest = Itmo.Dev.Asap.Github.Users.UpdateUsernameRequest;

namespace Itmo.Dev.Asap.Gateway.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private const string Scope = "Users";

    private readonly UserService.UserServiceClient _userClient;
    private readonly IdentityService.IdentityServiceClient _identityClient;
    private readonly IEnrichmentProcessor<string, UserDtoBuilder, UserDto> _enrichmentProcessor;
    private readonly GithubUserService.GithubUserServiceClient _githubUserClient;

    public UserController(
        UserService.UserServiceClient userClient,
        IdentityService.IdentityServiceClient identityClient,
        IEnrichmentProcessor<string, UserDtoBuilder, UserDto> enrichmentProcessor,
        GithubUserService.GithubUserServiceClient githubUserClient)
    {
        _userClient = userClient;
        _identityClient = identityClient;
        _enrichmentProcessor = enrichmentProcessor;
        _githubUserClient = githubUserClient;
    }

    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(204)]
    [AuthorizeFeature(Scope, nameof(Find))]
    public async Task<ActionResult<UserDto>> Find(int? universityId, CancellationToken cancellationToken)
    {
        UserDto? user;

        if (universityId is not null)
        {
            user = await FindUserByUniversityIdAsync(universityId.Value, cancellationToken);
        }
        else
        {
            user = await FindCurrentUserAsync(cancellationToken);
        }

        return user is not null ? Ok(user) : NoContent();
    }

    [HttpPut("{userId:guid}/universityId")]
    [AuthorizeFeature(Scope, nameof(UpdateUniversityId))]
    public async Task<ActionResult<UserDto>> UpdateUniversityId(
        Guid userId,
        int universityId,
        CancellationToken cancellationToken)
    {
        var request = new UpdateUniversityIdRequest { UserId = userId.ToString(), UniversityId = universityId };

        UpdateUniversityIdResponse? response = await _userClient
            .UpdateUniversityIdAsync(request, cancellationToken: cancellationToken);

        UserDtoBuilder[] builders = { response.User.ToBuilder() };
        IEnumerable<UserDto> users = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        return Ok(users.Single());
    }

    [HttpPut("{userId:guid}/name")]
    [AuthorizeFeature(Scope, nameof(UpdateName))]
    public async Task<ActionResult<UserDto>> UpdateName(
        Guid userId,
        UpdateNameRequest request,
        CancellationToken cancellationToken)
    {
        var grpcRequest = new Asap.Core.Users.UpdateNameRequest
        {
            UserId = userId.ToString(),
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
        };

        UpdateNameResponse response = await _userClient
            .UpdateNameAsync(grpcRequest, cancellationToken: cancellationToken);

        UserDtoBuilder[] builders = { response.User.ToBuilder() };
        IEnumerable<UserDto> users = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        return Ok(users.Single());
    }

    [AuthorizeFeature(Scope, nameof(UpdateGithubUsername))]
    [HttpPut("github/username")]
    public async Task<ActionResult> UpdateGithubUsername(
        [FromBody] UpdateGithubUsernameRequest request,
        CancellationToken cancellationToken)
    {
        UpdateUsernameRequest grpcRequest = request.MapToProtoRequest();
        await _githubUserClient.UpdateUsernameAsync(grpcRequest, cancellationToken: cancellationToken);

        return Ok();
    }

    [ProducesResponseType(200)]
    [HttpPost("identityInfo/query")]
    [AuthorizeFeature(Scope, nameof(QueryIdentityInfo))]
    public async Task<ActionResult<QueryUserIdentityInfoResponse>> QueryIdentityInfo(
        [FromBody] QueryUserIdentityInfoRequest request,
        CancellationToken cancellationToken)
    {
        var queryRequest = new QueryRequest
        {
            PageToken = request.PageToken,
            PageSize = request.PageSize,
            NamePatterns = { request.NamePatterns },
            UniversityIds = { request.UniversityIds },
        };

        QueryResponse? queryResponse = await _userClient.QueryAsync(queryRequest, cancellationToken: cancellationToken);

        var identityQuery = new FindUsersRequest
        {
            UserIds = { queryResponse.Users.Select(x => x.Id) },
        };

        FindUsersResponse identityResponse = await _identityClient
            .FindUsersAsync(identityQuery, cancellationToken: cancellationToken);

        IEnumerable<UserDtoBuilder> builders = queryResponse.Users.Select(x => x.ToBuilder());
        IEnumerable<UserDto> users = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        IEnumerable<UserIdentityInfoDto> result = users
            .GroupJoin(
                identityResponse.Users,
                x => x.Id,
                x => Guid.Parse(x.Id),
                (user, identity) => new UserIdentityInfoDto(user, identity.Any()));

        var response = new QueryUserIdentityInfoResponse(result, queryResponse.PageToken);

        return Ok(response);
    }

    private async Task<UserDto?> FindCurrentUserAsync(CancellationToken cancellationToken)
    {
        var request = new FindByIdRequest { UserId = HttpContext.User.GetId().ToString() };
        FindByIdResponse response = await _userClient.FindByIdAsync(request, cancellationToken: cancellationToken);

        if (response.UserCase is not FindByIdResponse.UserOneofCase.UserValue)
            return null;

        UserDtoBuilder[] builders = { response.UserValue.ToBuilder() };
        IEnumerable<UserDto> users = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        return users.Single();
    }

    private async Task<UserDto?> FindUserByUniversityIdAsync(int universityId, CancellationToken cancellationToken)
    {
        var request = new FindByUniversityIdRequest { UniversityId = universityId };

        FindByUniversityIdResponse response = await _userClient
            .FindByUniversityIdAsync(request, cancellationToken: cancellationToken);

        if (response.UserCase is not FindByUniversityIdResponse.UserOneofCase.UserValue)
            return null;

        UserDtoBuilder[] builders = { response.UserValue.ToBuilder() };
        IEnumerable<UserDto> users = await _enrichmentProcessor.EnrichAsync(builders, cancellationToken);

        return users.Single();
    }
}