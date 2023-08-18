using AutoMapper;
using Models;
using Models.DTOs.Auth;
using Models.DTOs.Like;
using Models.DTOs.Member;
using Models.DTOs.Message;
using Models.DTOs.Photo;

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

            CreateMap<Message, MessageDto>()
                .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<DateTime, DateTime>()
                .ConstructUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}
