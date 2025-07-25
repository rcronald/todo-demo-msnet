using TodoApp.TaskService.Domain.Entities;

namespace TodoApp.TaskService.Domain.Repositories;

public interface ITaskRepository
{
   System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetTasksByUserIdAsync(Guid userId, string? category = null, string? tag = null, bool? isCompleted = null);
   System.Threading.Tasks.Task<Entities.Task?> GetTaskByIdAsync(Guid id, Guid userId);
   System.Threading.Tasks.Task<Entities.Task> CreateTaskAsync(Entities.Task task);
   System.Threading.Tasks.Task<Entities.Task> UpdateTaskAsync(Entities.Task task);
   System.Threading.Tasks.Task DeleteTaskAsync(Guid id, Guid userId);
   System.Threading.Tasks.Task<bool> TaskExistsAsync(Guid id, Guid userId);
}

public interface ICategoryRepository
{
   System.Threading.Tasks.Task<IEnumerable<Category>> GetCategoriesAsync();
   System.Threading.Tasks.Task<Category?> GetCategoryByIdAsync(Guid id);
}

public interface ITagRepository
{
   System.Threading.Tasks.Task<IEnumerable<Tag>> GetTagsByUserIdAsync(Guid userId);
   System.Threading.Tasks.Task<Tag?> GetTagByNameAsync(string name, Guid userId);
   System.Threading.Tasks.Task<Tag> CreateTagAsync(Tag tag);
   System.Threading.Tasks.Task<IEnumerable<Tag>> GetOrCreateTagsAsync(IEnumerable<string> tagNames, Guid userId);
}

public interface IUserRepository
{
   System.Threading.Tasks.Task<User?> GetUserByKeycloakIdAsync(string keycloakUserId);
   System.Threading.Tasks.Task<User> CreateUserAsync(User user);
   System.Threading.Tasks.Task<User> UpdateUserAsync(User user);
}
