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
        }
    }
}
