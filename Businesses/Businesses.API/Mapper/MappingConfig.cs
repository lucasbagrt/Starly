using AutoMapper;
using Businesses.Domain.Dtos;
using Businesses.Domain.Entities;
using Newtonsoft.Json;

namespace Businesses.API.Mapper;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<Business, BusinessDto>()
                  .ForMember(dest => dest.Location, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<BusinessLocationDto>(src.Location)))
                  .ReverseMap();
            config.CreateMap<Business, BusinessByIdResponseDto>()
                  .ForMember(dest => dest.Location, opt => opt.MapFrom(src => JsonConvert.DeserializeObject<BusinessLocationDto>(src.Location)))
                  .ReverseMap();
            config.CreateMap<CategoryDto, Category>().ReverseMap();
            config.CreateMap<CreateCategoryDto, Category>().ReverseMap();
            config.CreateMap<CreateBusinessCategoryDto, BusinessCategory>().ReverseMap();
            config.CreateMap<CreateBusinessHourDto, BusinessHour>().ReverseMap();
            config.CreateMap<CreateBusinessDto, UpdateBusinessDto>().ReverseMap();
            config.CreateMap<UpdateBusinessDto, Business>()
                  .ForMember(dest => dest.Location, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Location)))
                  .ReverseMap();
            config.CreateMap<BusinessCategoryDto, BusinessCategory>().ReverseMap();
            config.CreateMap<UpdateBusinessCategoryDto, BusinessCategory>().ReverseMap();
            config.CreateMap<CreateBusinessDto, Business>()
                  .ForMember(dest => dest.Location, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.Location)))
                  .ReverseMap();

        });
        return mappingConfig;
    }
}