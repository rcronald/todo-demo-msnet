using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.TaskService.Domain.Repositories;
using TodoApp.Shared.Contracts.Responses;

namespace TodoApp.TaskService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<CategoryResponse>>>> GetCategories()
    {
        try
        {
            var categories = await _categoryRepository.GetCategoriesAsync();
            var categoryResponses = _mapper.Map<List<CategoryResponse>>(categories);
            
            return Ok(ApiResponse<List<CategoryResponse>>.SuccessResult(categoryResponses));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<CategoryResponse>>.ErrorResult("An error occurred while retrieving categories"));
        }
    }
}
