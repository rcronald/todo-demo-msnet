using TodoApp.TaskService.Domain.Entities;
using TodoApp.TaskService.Domain.Repositories;
using TodoApp.TaskService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.TaskService.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskDbContext _context;

    public TaskRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Domain.Entities.Task>> GetTasksByUserIdAsync(Guid userId, string? category = null, string? tag = null, bool? isCompleted = null)
    {
        var query = _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .Where(t => t.UserId == userId);

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(t => t.Category != null && t.Category.Name.ToLower() == category.ToLower());
        }

        if (!string.IsNullOrEmpty(tag))
        {
            query = query.Where(t => t.TaskTags.Any(tt => tt.Tag.Name.ToLower() == tag.ToLower()));
        }

        if (isCompleted.HasValue)
        {
            query = query.Where(t => t.IsCompleted == isCompleted.Value);
        }

        return await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
    }

    public async System.Threading.Tasks.Task<Domain.Entities.Task?> GetTaskByIdAsync(Guid id, Guid userId)
    {
        return await _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.TaskTags)
                .ThenInclude(tt => tt.Tag)
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
    }

    public async System.Threading.Tasks.Task<Domain.Entities.Task> CreateTaskAsync(Domain.Entities.Task task)
    {
        task.Id = Guid.NewGuid();
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return await GetTaskByIdAsync(task.Id, task.UserId) ?? task;
    }

    public async System.Threading.Tasks.Task<Domain.Entities.Task> UpdateTaskAsync(Domain.Entities.Task task)
    {
        task.UpdatedAt = DateTime.UtcNow;

        _context.Entry(task).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return await GetTaskByIdAsync(task.Id, task.UserId) ?? task;
    }

    public async System.Threading.Tasks.Task DeleteTaskAsync(Guid id, Guid userId)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async System.Threading.Tasks.Task<bool> TaskExistsAsync(Guid id, Guid userId)
    {
        return await _context.Tasks.AnyAsync(t => t.Id == id && t.UserId == userId);
    }
}

public class CategoryRepository : ICategoryRepository
{
    private readonly TaskDbContext _context;

    public CategoryRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
    }

    public async System.Threading.Tasks.Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }
}

public class TagRepository : ITagRepository
{
    private readonly TaskDbContext _context;

    public TagRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Tag>> GetTagsByUserIdAsync(Guid userId)
    {
        return await _context.Tags
            .Where(t => t.UserId == userId)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<Tag?> GetTagByNameAsync(string name, Guid userId)
    {
        return await _context.Tags
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower() && t.UserId == userId);
    }

    public async System.Threading.Tasks.Task<Tag> CreateTagAsync(Tag tag)
    {
        tag.Id = Guid.NewGuid();
        tag.CreatedAt = DateTime.UtcNow;

        _context.Tags.Add(tag);
        await _context.SaveChangesAsync();

        return tag;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Tag>> GetOrCreateTagsAsync(IEnumerable<string> tagNames, Guid userId)
    {
        var tags = new List<Tag>();

        foreach (var tagName in tagNames.Where(name => !string.IsNullOrWhiteSpace(name)))
        {
            var existingTag = await GetTagByNameAsync(tagName.Trim(), userId);
            if (existingTag != null)
            {
                tags.Add(existingTag);
            }
            else
            {
                var newTag = await CreateTagAsync(new Tag
                {
                    Name = tagName.Trim(),
                    UserId = userId
                });
                tags.Add(newTag);
            }
        }

        return tags;
    }
}

public class UserRepository : IUserRepository
{
    private readonly TaskDbContext _context;

    public UserRepository(TaskDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<User?> GetUserByKeycloakIdAsync(string keycloakUserId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.KeycloakUserId == keycloakUserId);
    }

    public async System.Threading.Tasks.Task<User> CreateUserAsync(User user)
    {
        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async System.Threading.Tasks.Task<User> UpdateUserAsync(User user)
    {
        user.UpdatedAt = DateTime.UtcNow;

        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return user;
    }
}
