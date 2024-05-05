using AutoMapper;
using Businesses.Domain.Dtos;
using Businesses.Domain.Entities;

namespace Businesses.API.Mapper;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<BusinessResponseDto, Business>().ReverseMap();
            config.CreateMap<BusinessByIdResponseDto, Business>().ReverseMap();
            config.CreateMap<CategoryDto, Category>().ReverseMap();
        });
        return mappingConfig;
    }
}