using System.ComponentModel.DataAnnotations;

namespace TodoApp.Shared.Contracts.Requests;

public class CreateUserProfileRequest
{
    [Required]
    [StringLength(255)]
    public string KeycloakUserId { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
}

public class UpdateUserProfileRequest
{
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
}
