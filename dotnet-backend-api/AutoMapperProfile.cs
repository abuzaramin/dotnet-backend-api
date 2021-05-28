using System;
using dotnet_backend_api.Dtos.Marvel;
using dotnet_backend_api.Models;
using AutoMapper;

namespace dotnet_backend_api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Marvel, GetMarvelDto>();
            CreateMap<AddMarvelDto, Marvel>();
        }
    }
   
}
