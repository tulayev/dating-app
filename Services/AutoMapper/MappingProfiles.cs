using AutoMapper;
using Models;
using Models.DTOs;

namespace Services.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain).Url));
            
            CreateMap<Photo, PhotoDto>();
            
            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<RegisterDto, AppUser>();

            CreateMap<AppUser, LikeDto>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id)) 
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain).Url)) 
                .ForMember(d => d.Age, o => o.MapFrom(s => s.GetAge())); 
        }
    }
}
