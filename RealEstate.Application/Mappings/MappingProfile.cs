using AutoMapper;
using RealEstate.Domain.Entities;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Property
            CreateMap<Property, PropertyDto>().ReverseMap();
            CreateMap<PropertyCreateDto, Property>();
            CreateMap<PropertyUpdateDto, Property>();

            // PropertyImage
            CreateMap<PropertyImage, PropertyImageDto>().ReverseMap();
            CreateMap<PropertyImageCreateDto, PropertyImage>();

            // PropertyTrace
            CreateMap<PropertyTrace, PropertyTraceDto>().ReverseMap();
        }
    }
}
