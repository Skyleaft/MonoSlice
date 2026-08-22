using Sannr;
using MonoSlice.Shared.Abstractions.Common;
using MonoSlice.Shared.Abstractions.CQRS;

namespace MonoSlice.Modules.Users.Features.Register;

public sealed partial class RegisterCommand : ICommand<ApiResponse<UserResponseDto>>
{
    [Required]
    [EmailAddress]
    [Sanitize(ToLower =  true)]
    public string Email { get; init; } = string.Empty;

    [Required]
    [Sanitize(Trim =  true)]
    [StringLength(50, MinimumLength = 3)]
    public string UserName { get; init; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; init; } = string.Empty;

    public string? FirstName { get; init; }
    public string? LastName { get; init; }
}

public sealed record UserResponseDto(
    Guid Id,
    string UserName,
    string Email,
    string? FirstName,
    string? LastName,
    IReadOnlyList<string> Roles,
    DateTime CreatedAt);
