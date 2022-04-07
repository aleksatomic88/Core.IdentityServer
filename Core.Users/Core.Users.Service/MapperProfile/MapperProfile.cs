using AutoMapper;
using Core.Users.DAL.Entities;
using Core.Users.Domain;

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
            #region User

            CreateMap<WeatherForecasts, UserResponse>();

            #endregion
        }

    }
}
