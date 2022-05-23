using AquaWater.Domain.Entities;
using AquaWater.Dto.Common;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using System;
using System.Linq;

namespace AquaWater.Dto.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CompanyManager, CompanyManagerRequestDTO>().ReverseMap();
            CreateMap<Customer, CustomerRequestDTO>().ReverseMap();
            CreateMap<Location, LocationRequestDTO>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();
            CreateMap<User, UserRegistrationRequestDTO>().ReverseMap()
            .ForMember(x => x.Location, y => y.MapFrom(x => x.Location));
            CreateMap<Customer, CustomerRequestDTO>().ReverseMap()
            .ForMember(x => x.User, y => y.MapFrom(x => x.User));

            CreateMap<Company, CompanyManagerRequestDTO>().ReverseMap();
            CreateMap<Notification, NotificationRequestDTO>().ReverseMap();
            CreateMap<OrderRequest, Order>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem));
            CreateMap<CreateOrderItem, OrderItem>();
            CreateMap<OrderItemRequestDTO, OrderItem>()
                .ForMember(x => x.Orders, y => y.Ignore())
                .ForMember(x => x.Product, y => y.Ignore());
            CreateMap<Order, OrderRequest>();
            CreateMap<OrderRequest, Order>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem))
                .ForMember(x => x.Transaction, y => y.Ignore())
                .ForMember(x => x.Customer, y => y.Ignore());
            CreateMap<OrderItem, OrderItemResponseDTO>();
            CreateMap<OrderResponse, Order>()
                .ForMember(x => x.OrderItem, y => y.MapFrom(x => x.OrderItem))
                .ReverseMap();

            CreateMap<Company, CompanyResponseDTO>()
                .ForMember(x => x.Location, y => y.MapFrom(z => z.Location))
                .ForMember(x => x.Product, y => y.MapFrom(z => z.Product.FirstOrDefault()));

            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Location, LocationResponseDTO>();
            CreateMap<Review, ReviewResponseDTO>().ReverseMap();
            CreateMap<ProductGallery, ImageResponseDTO>().ReverseMap();
            CreateMap<Product, ProductResponseDTO>()
            .ForMember(x => x.Photos, y => y.MapFrom(z => z.ProductGallery))
            .ForMember(x => x.Reviews, y => y.MapFrom(z => z.Review))
            .ReverseMap();

            CreateMap<Order, OrderResponse>();
            CreateMap<Product, ProductDTO>()
                .ForMember(x => x.Photos, y => y.MapFrom(z => z.ProductGallery))
                .ForMember(x => x.Rating, y => y.MapFrom(z => z.Rating.Count > 0? Math.Round(z.Rating.Average(x=>x.Rate), 1) : 0))
                .ForMember(x=>x.NoOfRating,y=> y.MapFrom(z=>z.Rating.Count()));
          
        }
    }
}
