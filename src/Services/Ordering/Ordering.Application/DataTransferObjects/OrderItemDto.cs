namespace Ordering.Application.DataTransferObjects
{
    public record OrderItemDto(Guid OrderId, Guid ProductId, int Quantity, decimal Price);
}
