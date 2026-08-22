using Sannr;
using MonoSlice.Shared.Abstractions.Common;
using MonoSlice.Shared.Abstractions.CQRS;

namespace MonoSlice.Modules.Orders.Features.CancelOrder;

public sealed partial class CancelOrderCommand : ICommand<ApiResponse<string>>
{
    public Guid OrderId { get; init; }

    [StringLength(200)]
    public string? Reason { get; init; }

    public CancelOrderCommand() { }

    public CancelOrderCommand(Guid orderId, string? reason = null)
    {
        OrderId = orderId;
        Reason = reason;
    }
}
