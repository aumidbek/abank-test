using Asakabank.UserApi.Entities;
using Asakabank.UserApi.Models;
using AutoMapper;

namespace Asakabank.UserApi.Profiles {
    public class MainProfile : Profile {
        public MainProfile() {
            CreateMap<DbUser, User>();
            CreateMap<User, DbUser>();
        }
    }
}