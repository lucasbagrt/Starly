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
            config.CreateMap<BusinessDto, Business>().ReverseMap();
            config.CreateMap<BusinessByIdResponseDto, Business>().ReverseMap();
            config.CreateMap<CategoryDto, Category>().ReverseMap();
            config.CreateMap<CreateCategoryDto, Category>().ReverseMap();
            config.CreateMap<CreateBusinessCategoryDto, BusinessCategory>().ReverseMap();
            config.CreateMap<CreateBusinessHourDto, BusinessHour>().ReverseMap();
            config.CreateMap<CreateBusinessDto, UpdateBusinessDto>().ReverseMap();
        });
        return mappingConfig;
    }
}