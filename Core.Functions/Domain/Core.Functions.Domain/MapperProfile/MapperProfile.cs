using AutoMapper;
using User.Functions.Domain.Model;

namespace User.Functions.Domain.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMapper();
        }
        private void CreateMapper()
        {
            CreateMap<UserVerificationEmail, Mailjet.Client.MailjetRequest>();
        }
    }
}
