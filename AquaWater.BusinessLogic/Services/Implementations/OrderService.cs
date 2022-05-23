using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Repository;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IGenericRepo<Order> _orderRepository;
        private readonly IGenericRepo<Product> _productRepository;
        private readonly IApplicationUser _findAppUser;
        private readonly IMapper _mapper;

        public OrderService(IGenericRepo<Order> genericRepo, IMapper mapper, IGenericRepo<Product> productRepository, IApplicationUser findAppUser)
        {
            _orderRepository = genericRepo;
            _mapper = mapper;
            _productRepository = productRepository;
            _findAppUser = findAppUser;
        }

        public async Task<OrderResponse> GetOrderByIdAsync(String id)
        {
            var order = await _orderRepository.TableNoTracking
                            .Where(o => o.Id == Guid.Parse(id))
                            .Include(o => o.OrderItem).FirstOrDefaultAsync();

            var map = _mapper.Map<OrderResponse>(order);
            return map;
        }

        public async Task<Response<OrderItemRequestDTO>> CreateOrderAsync(OrderRequest createOrderRequest, string id)
        {
            var customer = await _findAppUser.GetCustomerIdByUserId(id);
            Order order = _mapper.Map<Order>(createOrderRequest);
            order.CustomerId = customer.Id;
            order.OrderStatus = (int)OrderStatus.OnHold;
            order.OrderDate = DateTime.Now;

            string error = string.Empty;
            foreach (var item in createOrderRequest.OrderItem)
            {
                var product = await _productRepository.GetByIdAysnc(item.ProductId);
                if (product == null)
                {
                    error += $"Product with Id {product.Id} \\n\\";
                    continue;
                }
                var orderItem = order.OrderItem.FirstOrDefault(x => x.ProductId == item.ProductId);

                orderItem.CurrentProductPrice = product.Price - (product.Price * (product.Discount / 100));
                orderItem.Quantity = item.Quantity;


                return new Response<OrderItemRequestDTO>()
                {
                    Errors = error,
                    Message = "Order Created Unsuccessfully",
                    Success = false,
                };
            }

            await _orderRepository.InsertAsync(order);
            return new Response<OrderItemRequestDTO>()
            {
                Errors = null,
                Message = "Order Created Successfully",
                Success = true,
            };

            
        }

        public async Task<Response<string>> VerifyOTP(VerifyOtpDTO verifyOtp)
        {
            var order = await _orderRepository.GetByIdAysnc(Guid.Parse(verifyOtp.OrderId));
            if (order.OTP != verifyOtp.OTP)
            {
                return new Response<string>
                {
                    Success = false,
                    Message = "OTP verification unsuccessful"
                };
            }
            return new Response<string>
            {
                Success = true,
                Message = "OTP verification successful"
            };
        }
    }
}
