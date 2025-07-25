using System.ComponentModel.DataAnnotations;

namespace TodoApp.Shared.Contracts.Requests;

public class CreateTaskRequest
{
    [Required]
    [StringLength(255, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid? CategoryId { get; set; }

    [StringLength(10)]
    public string Priority { get; set; } = "Medium";

    public List<string> Tags { get; set; } = new();
}

public class UpdateTaskRequest
{
    [Required]
    [StringLength(255, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public Guid? CategoryId { get; set; }

    [StringLength(10)]
    public string Priority { get; set; } = "Medium";

    public List<string> Tags { get; set; } = new();
}

public class UpdateTaskStatusRequest
{
    public bool IsCompleted { get; set; }
}

public class CreateTagRequest
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
}
