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
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public TasksController(
        ITaskRepository taskRepository,
        ITagRepository tagRepository,
        IUserRepository userRepository,
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _tagRepository = tagRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    private async Task<Guid> GetUserIdAsync()
    {
        var keycloakUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(keycloakUserId))
            throw new UnauthorizedAccessException("User not authenticated");

        var user = await _userRepository.GetUserByKeycloakIdAsync(keycloakUserId);
        if (user == null)
            throw new UnauthorizedAccessException("User not found");

        return user.Id;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TaskResponse>>>> GetTasks(
        [FromQuery] string? category = null,
        [FromQuery] string? tag = null,
        [FromQuery] bool? isCompleted = null)
    {
        try
        {
            var userId = await GetUserIdAsync();
            var tasks = await _taskRepository.GetTasksByUserIdAsync(userId, category, tag, isCompleted);
            var taskResponses = _mapper.Map<List<TaskResponse>>(tasks);
            
            return Ok(ApiResponse<List<TaskResponse>>.SuccessResult(taskResponses));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<List<TaskResponse>>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<TaskResponse>>.ErrorResult("An error occurred while retrieving tasks"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TaskResponse>>> GetTask(Guid id)
    {
        try
        {
            var userId = await GetUserIdAsync();
            var task = await _taskRepository.GetTaskByIdAsync(id, userId);
            
            if (task == null)
                return NotFound(ApiResponse<TaskResponse>.ErrorResult("Task not found"));

            var taskResponse = _mapper.Map<TaskResponse>(task);
            return Ok(ApiResponse<TaskResponse>.SuccessResult(taskResponse));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<TaskResponse>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TaskResponse>.ErrorResult("An error occurred while retrieving the task"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TaskResponse>>> CreateTask(CreateTaskRequest request)
    {
        try
        {
            var userId = await GetUserIdAsync();
            var task = _mapper.Map<Domain.Entities.Task>(request);
            task.UserId = userId;

            // Handle tags
            if (request.Tags.Any())
            {
                var tags = await _tagRepository.GetOrCreateTagsAsync(request.Tags, userId);
                task.TaskTags = tags.Select(tag => new Domain.Entities.TaskTag
                {
                    TagId = tag.Id,
                    CreatedAt = DateTime.UtcNow
                }).ToList();
            }

            var createdTask = await _taskRepository.CreateTaskAsync(task);
            var taskResponse = _mapper.Map<TaskResponse>(createdTask);
            
            return CreatedAtAction(nameof(GetTask), new { id = createdTask.Id }, 
                ApiResponse<TaskResponse>.SuccessResult(taskResponse, "Task created successfully"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<TaskResponse>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TaskResponse>.ErrorResult("An error occurred while creating the task"));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TaskResponse>>> UpdateTask(Guid id, UpdateTaskRequest request)
    {
        try
        {
            var userId = await GetUserIdAsync();
            var existingTask = await _taskRepository.GetTaskByIdAsync(id, userId);
            
            if (existingTask == null)
                return NotFound(ApiResponse<TaskResponse>.ErrorResult("Task not found"));

            // Update task properties
            existingTask.Title = request.Title;
            existingTask.Description = request.Description;
            existingTask.DueDate = request.DueDate;
            existingTask.CategoryId = request.CategoryId;
            existingTask.Priority = request.Priority;

            // Update tags
            existingTask.TaskTags.Clear();
            if (request.Tags.Any())
            {
                var tags = await _tagRepository.GetOrCreateTagsAsync(request.Tags, userId);
                existingTask.TaskTags = tags.Select(tag => new Domain.Entities.TaskTag
                {
                    TaskId = id,
                    TagId = tag.Id,
                    CreatedAt = DateTime.UtcNow
                }).ToList();
            }

            var updatedTask = await _taskRepository.UpdateTaskAsync(existingTask);
            var taskResponse = _mapper.Map<TaskResponse>(updatedTask);
            
            return Ok(ApiResponse<TaskResponse>.SuccessResult(taskResponse, "Task updated successfully"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<TaskResponse>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TaskResponse>.ErrorResult("An error occurred while updating the task"));
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<ApiResponse<TaskResponse>>> UpdateTaskStatus(Guid id, UpdateTaskStatusRequest request)
    {
        try
        {
            var userId = await GetUserIdAsync();
            var task = await _taskRepository.GetTaskByIdAsync(id, userId);
            
            if (task == null)
                return NotFound(ApiResponse<TaskResponse>.ErrorResult("Task not found"));

            task.IsCompleted = request.IsCompleted;
            var updatedTask = await _taskRepository.UpdateTaskAsync(task);
            var taskResponse = _mapper.Map<TaskResponse>(updatedTask);
            
            return Ok(ApiResponse<TaskResponse>.SuccessResult(taskResponse, "Task status updated successfully"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<TaskResponse>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<TaskResponse>.ErrorResult("An error occurred while updating task status"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> DeleteTask(Guid id)
    {
        try
        {
            var userId = await GetUserIdAsync();
            var taskExists = await _taskRepository.TaskExistsAsync(id, userId);
            
            if (!taskExists)
                return NotFound(ApiResponse<object>.ErrorResult("Task not found"));

            await _taskRepository.DeleteTaskAsync(id, userId);
            
            return Ok(ApiResponse<object>.SuccessResult(null, "Task deleted successfully"));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ApiResponse<object>.ErrorResult(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResult("An error occurred while deleting the task"));
        }
    }
}
