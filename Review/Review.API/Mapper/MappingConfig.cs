using AutoMapper;
using Review.Domain.Dtos;

namespace Review.API.Mapper;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<CreateReviewDto, Domain.Entities.Review>().ReverseMap();
            config.CreateMap<UpdateReviewDto, Domain.Entities.Review>().ReverseMap();
            config.CreateMap<ReviewDto, Domain.Entities.Review>().ReverseMap();
        });
        return mappingConfig;
    }
}