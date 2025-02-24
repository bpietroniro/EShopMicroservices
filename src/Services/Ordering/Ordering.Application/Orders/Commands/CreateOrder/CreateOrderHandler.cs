using BuildingBlocks.CQRS;

namespace Ordering.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            // create order entity from command object
            // save to database
            // return result
            throw new NotImplementedException();
        }
    }
}
