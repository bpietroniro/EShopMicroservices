using Discount.gRPC.Data;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Discount.gRPC.Models;

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

        public override async Task<CouponModel> CreateDiscount(
            CreateDiscountRequest request,
            ServerCallContext context
        )
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            }

            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount successfully created. ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<CouponModel> UpdateDiscount(
            UpdateDiscountRequest request,
            ServerCallContext context
        )
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object"));
            }

            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount successfully created. ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

            return coupon.Adapt<CouponModel>();
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(
            DeleteDiscountRequest request,
            ServerCallContext context
        )
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);

            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName {request.ProductName} not found."));
            }

            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount successfully deleted. ProductName: {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
