using AutoMapper;
using Core.Users.Domain;
using Core.Users.Service;

namespace Users.Core.Service.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMapper();
        }

        private void CreateMapper()
        {
            CreateMap<User, UserResponse>();
            CreateMap<User, UserBasicResponse>();

            CreateMap<Role, RoleResponse>();
        }
    }
}
