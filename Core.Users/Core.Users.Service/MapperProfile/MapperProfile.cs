using AutoMapper;
using Common.Model.ServiceBus;
using Core.Users.DAL.Entity;
using Core.Users.Service;
using HashidsNet;

namespace Users.Core.Service.MapperProfile
{
    public class MapperProfile : Profile
    {
        private readonly IHashids _hashids;

        public MapperProfile(IHashids hashids)
        {
            _hashids = hashids;
            CreateMapper();
        }

        private void CreateMapper()
        {
            CreateMap<User, UserResponse>().ForMember(dst => dst.Id, opt => opt.MapFrom(src => _hashids.Encode(src.Id)));
            CreateMap<User, UserBasicResponse>().ForMember(dst => dst.Id, opt => opt.MapFrom(src => _hashids.Encode(src.Id)));

            CreateMap<User, UserServiceBusMessageObject>()
                .ForMember(dst => dst.UserID, opt => opt.MapFrom(src => _hashids.Encode(src.Id)))
                .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.Token, opt => opt.MapFrom(src => src.VerificationToken != null ? src.VerificationToken : src.ResetToken))
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<Role, RoleResponse>();
        }
    }
}
