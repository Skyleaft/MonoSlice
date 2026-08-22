using Sannr;
using MonoSlice.Modules.Orders.Domain;
using MonoSlice.Shared.Abstractions.Common;
using MonoSlice.Shared.Abstractions.CQRS;

namespace MonoSlice.Modules.Orders.Features.CreateOrder;

public sealed partial class CreateOrderItemDto
{
    [Required]
    public Guid ProductId { get; init; }

    [Range(1, 10000, ErrorMessage = "Quantity must be between 1 and 10,000")]
    public int Quantity { get; init; }

    public CreateOrderItemDto() { }

    public CreateOrderItemDto(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}

public sealed partial class CreateOrderCommand : ICommand<ApiResponse<OrderDto>>
{
    [Required]
    public Guid CustomerId { get; init; }

    [Required]
    public List<CreateOrderItemDto> Items { get; init; } = [];

    [StringLength(500)]
    public string? Notes { get; init; }

    public bool AutoProcessAsync { get; init; } = true;

    public CreateOrderCommand() { }

    public CreateOrderCommand(Guid customerId, List<CreateOrderItemDto> items, string? notes = null, bool autoProcessAsync = true)
    {
        CustomerId = customerId;
        Items = items;
        Notes = notes;
        AutoProcessAsync = autoProcessAsync;
    }
}

public sealed record OrderItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal TotalPrice);

public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    decimal TotalAmount,
    string? Notes,
    IReadOnlyList<OrderItemDto> Items,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
