using Microsoft.AspNetCore.Identity;
using MonoSlice.Modules.Users.Domain;
using MonoSlice.Modules.Users.Features.Register;
using MonoSlice.Shared.Abstractions.Common;
using MonoSlice.Shared.Abstractions.CQRS;
using MonoSlice.Shared.Abstractions.Exceptions;
using MonoSlice.Shared.Abstractions.Interfaces;

namespace MonoSlice.Modules.Users.Features.GetProfile;

public sealed class GetProfileQueryHandler : IQueryHandler<GetProfileQuery, ApiResponse<UserResponseDto>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICurrentUser _currentUser;

    public GetProfileQueryHandler(
        UserManager<ApplicationUser> userManager,
        ICurrentUser currentUser)
    {
        _userManager = userManager;
        _currentUser = currentUser;
    }

    public async ValueTask<ApiResponse<UserResponseDto>> Handle(
        GetProfileQuery query,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated || !_currentUser.UserId.HasValue)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var user = await _userManager.FindByIdAsync(_currentUser.UserId.Value.ToString());
        if (user is null)
        {
            throw new NotFoundException(nameof(ApplicationUser), _currentUser.UserId.Value);
        }

        var roles = await _userManager.GetRolesAsync(user);

        var dto = new UserResponseDto(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            user.FirstName,
            user.LastName,
            roles.ToList(),
            user.CreatedAt);

        return ApiResponse.Ok(dto);
    }
}
