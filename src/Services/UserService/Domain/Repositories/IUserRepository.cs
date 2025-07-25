using TodoApp.UserService.Domain.Entities;

namespace TodoApp.UserService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByKeycloakIdAsync(string keycloakUserId);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
}
