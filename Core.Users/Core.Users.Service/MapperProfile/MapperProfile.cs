using AutoMapper;
using Core.Users.Domain.Model;
using Core.Users.Domain.Response;

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
            CreateMap<Role, RoleResponse>();
        }

    }
}
