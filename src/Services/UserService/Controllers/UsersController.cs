using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoApp.UserService.Domain.Entities;
using TodoApp.UserService.Domain.Repositories;
using TodoApp.Shared.Contracts.Requests;
using TodoApp.Shared.Contracts.Responses;

namespace TodoApp.UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<ApiResponse<UserProfileResponse>>> GetProfile()
    {
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(keycloakUserId))
                return Unauthorized(ApiResponse<UserProfileResponse>.ErrorResult("User not authenticated"));

            var user = await _userRepository.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
                return NotFound(ApiResponse<UserProfileResponse>.ErrorResult("User profile not found"));

            var userResponse = _mapper.Map<UserProfileResponse>(user);
            return Ok(ApiResponse<UserProfileResponse>.SuccessResult(userResponse));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UserProfileResponse>.ErrorResult("An error occurred while retrieving user profile"));
        }
    }

    [HttpPost("profile")]
    public async Task<ActionResult<ApiResponse<UserProfileResponse>>> CreateProfile(CreateUserProfileRequest request)
    {
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(keycloakUserId))
                return Unauthorized(ApiResponse<UserProfileResponse>.ErrorResult("User not authenticated"));

            // Check if user already exists
            var existingUser = await _userRepository.GetUserByKeycloakIdAsync(keycloakUserId);
            if (existingUser != null)
                return Conflict(ApiResponse<UserProfileResponse>.ErrorResult("User profile already exists"));

            var user = new User
            {
                KeycloakUserId = keycloakUserId,
                Username = request.Username,
                Email = request.Email
            };

            var createdUser = await _userRepository.CreateUserAsync(user);
            var userResponse = _mapper.Map<UserProfileResponse>(createdUser);

            return CreatedAtAction(nameof(GetProfile), 
                ApiResponse<UserProfileResponse>.SuccessResult(userResponse, "User profile created successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UserProfileResponse>.ErrorResult("An error occurred while creating user profile"));
        }
    }

    [HttpPut("profile")]
    public async Task<ActionResult<ApiResponse<UserProfileResponse>>> UpdateProfile(UpdateUserProfileRequest request)
    {
        try
        {
            var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(keycloakUserId))
                return Unauthorized(ApiResponse<UserProfileResponse>.ErrorResult("User not authenticated"));

            var user = await _userRepository.GetUserByKeycloakIdAsync(keycloakUserId);
            if (user == null)
                return NotFound(ApiResponse<UserProfileResponse>.ErrorResult("User profile not found"));

            user.Username = request.Username;
            user.Email = request.Email;

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            var userResponse = _mapper.Map<UserProfileResponse>(updatedUser);

            return Ok(ApiResponse<UserProfileResponse>.SuccessResult(userResponse, "User profile updated successfully"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<UserProfileResponse>.ErrorResult("An error occurred while updating user profile"));
        }
    }
}
