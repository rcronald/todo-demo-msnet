using AutoMapper;
using TodoApp.UserService.Domain.Entities;
using TodoApp.Shared.Contracts.Responses;

namespace TodoApp.UserService.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserProfileResponse>();
    }
}
