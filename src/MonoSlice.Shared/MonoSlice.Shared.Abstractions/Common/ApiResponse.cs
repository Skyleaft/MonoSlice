namespace MonoSlice.Shared.Abstractions.Common;

/// <summary>
/// Standard API response wrapper for all endpoints.
/// </summary>
public sealed class ApiResponse
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public IReadOnlyList<string>? Errors { get; init; }

    public static ApiResponse Ok(string? message = null) =>
        new() { Success = true, Message = message };

    public static ApiResponse Fail(string message) =>
        new() { Success = false, Message = message };

    public static ApiResponse Fail(string message, IReadOnlyList<string> errors) =>
        new() { Success = false, Message = message, Errors = errors };

    public static ApiResponse<T> Ok<T>(T data, string? message = null) =>
        new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail<T>(string message) =>
        new() { Success = false, Message = message };

    public static ApiResponse<T> Fail<T>(string message, IReadOnlyList<string> errors) =>
        new() { Success = false, Message = message, Errors = errors };
}

/// <summary>
/// Standard API response wrapper with typed data payload.
/// </summary>
public sealed class ApiResponse<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public IReadOnlyList<string>? Errors { get; init; }
}
