using AutoMapper;
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

            CreateMap<Role, RoleResponse>();
        }
    }
}
