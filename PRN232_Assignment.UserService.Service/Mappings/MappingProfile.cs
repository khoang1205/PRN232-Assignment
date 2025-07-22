using AutoMapper;
using PRN232_Assignment.UserService.Repository.Entities;
using PRN232_Assignment.UserService.Service.Models;

namespace PRN232_Assignment.UserService.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>();
        }
    }
}
