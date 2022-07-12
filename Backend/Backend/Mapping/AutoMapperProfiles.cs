using AutoMapper;
using Backend.Dtos;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Mapping
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<User, UserDetailsDto>().ReverseMap();

            CreateMap<Order, PendingOrderDto>().ReverseMap();
        }
    }
}
