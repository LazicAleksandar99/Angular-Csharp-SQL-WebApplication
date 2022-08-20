using AutoMapper;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<User, UserDetailsDto>().ReverseMap();

            CreateMap<Order, PendingOrderDto>().ReverseMap();

            CreateMap<User, DeliverDto>().ReverseMap();

            CreateMap<Product, NewProductDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<User, StatusDto>().ReverseMap();

            CreateMap<Order, CurrentOrderDto>().ReverseMap();
        }
    }
}
