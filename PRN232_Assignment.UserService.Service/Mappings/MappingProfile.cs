using AutoMapper;
using PRN232_Assignment.UserService.Repository.Entities;
using PRN232_Assignment.UserService.Service.Models;
using EntityUser = PRN232_Assignment.UserService.Repository.Entities.User;
namespace PRN232_Assignment.UserService.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, EntityUser>();
        }
    }
}
