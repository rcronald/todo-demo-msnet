using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApp.TaskService.Domain.Repositories;
using TodoApp.Shared.Contracts.Requests;
using TodoApp.Shared.Contracts.Responses;

namespace TodoApp.TaskService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TagsController : ControllerBase
{
    private readonly ITagRepository _tagRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TagsController> _logger;

    public TagsController(
        ITagRepository tagRepository,
        IUserRepository userRepository,
        IMapper mapper,
        ILogger<TagsController> logger)
    {
        _tagRepository = tagRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    private async Task<Guid> GetUserIdAsync()
    {
        var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation("Retrieving user ID from claims {keycloakUserId}", keycloakUserId);
        if (string.IsNullOrEmpty(keycloakUserId))
            throw new UnauthorizedAccessException("User not authenticated");

        var user = await _userRepository.GetUserByKeycloakIdAsync(keycloakUserId);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");

        return user.Id;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TagResponse>>>> GetTags()
    {
        try
        {
            var userId = await GetUserIdAsync();
            _logger.LogInformation("Retrieving tags for user {UserId}", userId);
            var tags = await _tagRepository.GetTagsByUserIdAsync(userId);
            var tagResponses = _mapper.Map<List<TagResponse>>(tags);
            
            return Ok(ApiResponse<List<TagResponse>>.SuccessResult(tagResponses));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<List<TagResponse>>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<TagResponse>>.ErrorResult("An error occurred while retrieving tags"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TagResponse>>> CreateTag(CreateTagRequest request)
    {
        try
        {
            var userId = await GetUserIdAsync();
            
            // Check if tag already exists
            var existingTag = await _tagRepository.GetTagByNameAsync(request.Name, userId);
            if (existingTag != null)
                return Conflict(ApiResponse<TagResponse>.ErrorResult("Tag with this name already exists"));

            var tag = new Domain.Entities.Tag
            {
                Name = request.Name,
                UserId = userId
            };

            var createdTag = await _tagRepository.CreateTagAsync(tag);
            var tagResponse = _mapper.Map<TagResponse>(createdTag);
            
            return CreatedAtAction(nameof(GetTags), 
                ApiResponse<TagResponse>.SuccessResult(tagResponse, "Tag created successfully"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<TagResponse>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TagResponse>.ErrorResult("An error occurred while creating the tag"));
        }
    }
}
