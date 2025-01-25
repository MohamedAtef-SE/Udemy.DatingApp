using API.DTOs.members;
using API.Entities;
using API.Extentions;
using AutoMapper;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, MemberDTO>()
            .ForMember(memberDTO => memberDTO.Age,options => options.MapFrom(appUser => appUser.DateOfBirth.CalculateAge()))
            .ForMember(memberDTO => memberDTO.PhotoURL,options => options.MapFrom(appUser => appUser.Photos.FirstOrDefault(photo => photo.IsMain)!.URL));
            
            CreateMap<Photo,PhotoDTO>();
        }
    }
}