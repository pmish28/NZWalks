using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region,RegionDTO>().ReverseMap();
            CreateMap<AddRegionRequestDTO,Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDTO,Region>().ReverseMap();

            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<Walk,WalkDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto,Walk>().ReverseMap();

            CreateMap<Difficulty,DifficultyDto>().ReverseMap();
                
        }
    }
}
