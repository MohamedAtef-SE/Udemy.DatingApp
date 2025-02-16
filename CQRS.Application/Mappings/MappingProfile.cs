using API.Extentions;
using AutoMapper;
using CQRS.Application.DTOs.auth;
using CQRS.Application.DTOs.members;
using CQRS.Application.DTOs.messages;
using CQRS.Domain.Entities;

namespace CQRS.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, MemberDTO>()
            .ForMember(memberDTO => memberDTO.Age, options => options.MapFrom(appUser => appUser.DateOfBirth.CalculateAge()))
            .ForMember(memberDTO => memberDTO.PhotoURL, options => options.MapFrom(appUser => appUser.Photos.FirstOrDefault(photo => photo.IsMain)!.URL));

            CreateMap<Photo, PhotoDTO>();

            CreateMap<UpdateMemberDTO, AppUser>();
            CreateMap<RegisterDTO, AppUser>();
            CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));

            CreateMap<Message, MessageDTO>()
                .ForMember(des => des.RecipientPhotoURL,
                           o => o.MapFrom(src =>
                           src.Recipient.Photos.FirstOrDefault(p => p.IsMain)!.URL)
                           )
                .ForMember(des => des.SenderPhotoURL,
                          o => o.MapFrom(src =>
                          src.Sender.Photos.FirstOrDefault(p => p.IsMain)!.URL)
                          );

            //for read message date issue
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);

        }
    }
}