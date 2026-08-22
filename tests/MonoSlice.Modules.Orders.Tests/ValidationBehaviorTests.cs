using Sannr;
using Mediator;
using MonoSlice.Shared.Abstractions.Common;
using ValidationException = MonoSlice.Shared.Abstractions.Exceptions.ValidationException;
using Xunit;

namespace MonoSlice.Modules.Orders.Tests;

public sealed partial class ValidatableCommand : IMessage
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "Age must be between 1 and 100.")]
    public int Age { get; set; }
}

public sealed partial class NonResultCommand : IMessage
{
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; } = string.Empty;
}

public sealed class ValidationBehaviorTests
{
    [Fact]
    public void SannrValidatorRegistry_HasRegisteredValidator_ForValidatableCommand()
    {
        var found = SannrValidatorRegistry.TryGetValidator(typeof(ValidatableCommand), out var validator);
        Assert.True(found);
        Assert.NotNull(validator);
    }

    [Fact]
    public async Task Handle_ValidMessage_CallsNextDelegate()
    {
        // Arrange
        var behavior = new MonoSlice.Shared.Infrastructure.Behaviors.ValidationBehavior<ValidatableCommand, ApiResponse<string>>();
        var command = new ValidatableCommand { Name = "John Doe", Age = 25 };
        var expectedResponse = ApiResponse.Ok<string>("Success");

        MessageHandlerDelegate<ValidatableCommand, ApiResponse<string>> next =
            (msg, ct) => ValueTask.FromResult(expectedResponse);

        // Act
        var result = await behavior.Handle(command, next, CancellationToken.None);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("Success", result.Data);
    }

    [Fact]
    public async Task Handle_InvalidMessage_WithApiResponse_ReturnsFailureResult_WithoutThrowing()
    {
        // Arrange
        var behavior = new MonoSlice.Shared.Infrastructure.Behaviors.ValidationBehavior<ValidatableCommand, ApiResponse<string>>();
        var invalidCommand = new ValidatableCommand { Name = "", Age = -5 };

        var nextCalled = false;
        MessageHandlerDelegate<ValidatableCommand, ApiResponse<string>> next = (msg, ct) =>
        {
            nextCalled = true;
            return ValueTask.FromResult(ApiResponse.Ok<string>("Should not be called"));
        };

        // Act
        var result = await behavior.Handle(invalidCommand, next, CancellationToken.None);

        // Assert
        Assert.False(nextCalled);
        Assert.False(result.Success);
        Assert.Equal(400, result.StatusCode);
        Assert.Equal(400, result.Status);
        Assert.Equal("Validation Error", result.Title);
        Assert.Equal(ProblemTypes.ValidationError, result.Type);
        Assert.Equal("Validation.Error", result.Code);
        Assert.NotNull(result.Errors);
        Assert.NotEmpty(result.Errors);
        Assert.True(result.Errors.ContainsKey("Name"));
        Assert.True(result.Errors.ContainsKey("Age"));
        Assert.NotNull(result.ErrorList);
        Assert.Contains("Name is required.", result.ErrorList);
    }

    [Fact]
    public async Task Handle_InvalidMessage_WithResult_ReturnsResultFailureWithValidationErrors()
    {
        // Arrange
        var behavior = new MonoSlice.Shared.Infrastructure.Behaviors.ValidationBehavior<ValidatableCommand, Result<string>>();
        var invalidCommand = new ValidatableCommand { Name = "", Age = -5 };

        var nextCalled = false;
        MessageHandlerDelegate<ValidatableCommand, Result<string>> next = (msg, ct) =>
        {
            nextCalled = true;
            return ValueTask.FromResult(Result<string>.Success("Should not be called"));
        };

        // Act
        var result = await behavior.Handle(invalidCommand, next, CancellationToken.None);

        // Assert
        Assert.False(nextCalled);
        Assert.True(result.IsFailure);
        Assert.Equal(ErrorType.Validation, result.Error.Type);
        Assert.NotNull(result.ValidationErrors);
        Assert.True(result.ValidationErrors.ContainsKey("Name"));
        Assert.True(result.ValidationErrors.ContainsKey("Age"));
    }

    [Fact]
    public async Task Handle_InvalidMessage_WithNonResultResponse_ThrowsValidationException()
    {
        // Arrange
        var behavior = new MonoSlice.Shared.Infrastructure.Behaviors.ValidationBehavior<NonResultCommand, int>();
        var invalidCommand = new NonResultCommand { Title = "" };

        MessageHandlerDelegate<NonResultCommand, int> next = (msg, ct) => ValueTask.FromResult(42);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ValidationException>(() =>
            behavior.Handle(invalidCommand, next, CancellationToken.None).AsTask());

        Assert.NotEmpty(ex.Errors);
        Assert.Equal(400, ex.StatusCode);
        Assert.Equal("Validation Error", ex.Title);
        Assert.Equal(ProblemTypes.ValidationError, ex.Type);
        Assert.True(ex.Errors.ContainsKey("Title"));
    }
}
