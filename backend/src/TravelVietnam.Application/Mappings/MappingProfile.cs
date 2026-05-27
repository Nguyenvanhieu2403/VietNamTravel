using AutoMapper;
using TravelVietnam.Domain.Entities;
using TravelVietnam.Application.DTOs.Travel;

namespace TravelVietnam.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Region, RegionDto>()
                .ForMember(dest => dest.Provinces, opt => opt.MapFrom(src => src.Provinces));
                
            CreateMap<Province, ProvinceListDto>();
            
            CreateMap<Province, ProvinceDto>()
                .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region.Name))
                .ForMember(dest => dest.Destinations, opt => opt.MapFrom(src => src.Destinations))
                .ForMember(dest => dest.Foods, opt => opt.MapFrom(src => src.Foods))
                .ForMember(dest => dest.Festivals, opt => opt.MapFrom(src => src.Festivals))
                .ForMember(dest => dest.Seasons, opt => opt.MapFrom(src => src.Seasons))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews))
                .ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.MediaFiles));

            CreateMap<Destination, DestinationDto>()
                .ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.MediaFiles));

            CreateMap<Food, FoodDto>();
            CreateMap<Festival, FestivalDto>();
            CreateMap<TravelSeason, TravelSeasonDto>();
            CreateMap<MediaFile, MediaFileDto>();

            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName));

            CreateMap<Blog, BlogDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FullName))
                .ForMember(dest => dest.MediaFiles, opt => opt.MapFrom(src => src.MediaFiles));
        }
    }
}
