using Discount.gRPC;

namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;

    public record StoreBasketResult(string UserName);

    public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketCommandValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart cannot be null");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
        }
    }

    public class StoreBasketCommandHandler(
        IBasketRepository repository,
        DiscountProtoService.DiscountProtoServiceClient discountProto
    ) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(
            StoreBasketCommand command,
            CancellationToken cancellationToken
        )
        {
            ShoppingCart cart = command.Cart;

            await DeductDiscount(cart, cancellationToken);

            // store in database and update cache
            await repository.StoreBasket(cart, cancellationToken);

            return new StoreBasketResult(cart.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart, CancellationToken cancellationToken)
        {
            // communicate with discount gRPC service
            foreach (var item in cart.Items)
            {
                var coupon = await discountProto.GetDiscountAsync(
                    new GetDiscountRequest { ProductName = item.ProductName }
                );
                item.Price -= coupon.Amount;
            }
        }
    }
}
