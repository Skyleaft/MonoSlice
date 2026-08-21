using System.ComponentModel.DataAnnotations;
using Mediator;
using MonoSlice.Shared.Abstractions.Exceptions;

namespace MonoSlice.Shared.Infrastructure.Behaviors;

public sealed class ValidationBehavior<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext(message, serviceProvider: null, items: null);
        var validationResults = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(message, context, validationResults, validateAllProperties: true);

        if (!isValid)
        {
            var errors = validationResults
                .Select(r => r.ErrorMessage ?? "Validation error occurred.")
                .ToList();

            throw new MonoSlice.Shared.Abstractions.Exceptions.ValidationException(errors);
        }

        return await next(message, cancellationToken);
    }
}
