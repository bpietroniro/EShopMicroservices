using Discount.gRPC.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Discount.gRPC.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
        : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(
            GetDiscountRequest request,
            ServerCallContext context
        )
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon is null)
            {
                coupon = new Models.Coupon { ProductName = "No Discount", Amount = 0, Description = "No Description" };
            }

            logger.LogInformation("Discount retrieved for ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

            return coupon.Adapt<CouponModel>();
        }

        public override Task<CouponModel> CreateDiscount(
            CreateDiscountRequest request,
            ServerCallContext context
        )
        {
            return base.CreateDiscount(request, context);
        }

        public override Task<CouponModel> UpdateDiscount(
            UpdateDiscountRequest request,
            ServerCallContext context
        )
        {
            return base.UpdateDiscount(request, context);
        }

        public override Task<DeleteDiscountResponse> DeleteDiscount(
            DeleteDiscountRequest request,
            ServerCallContext context
        )
        {
            return base.DeleteDiscount(request, context);
        }
    }
}
