using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Response<string>> VerifyOTP(VerifyOtpDTO verifyOtp);


        public Task<Response<OrderItemRequestDTO>> CreateOrderAsync(OrderRequest createOrderRequest, string id);
        public  Task<OrderResponse> GetOrderByIdAsync(String id);

    }
}
