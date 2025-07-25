using AutoMapper;
using TodoApp.TaskService.Domain.Entities;
using TodoApp.Shared.Contracts.Requests;
using TodoApp.Shared.Contracts.Responses;

namespace TodoApp.TaskService.Application.Mappings;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<Domain.Entities.Task, TaskResponse>()
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.TaskTags.Select(tt => tt.Tag)));

        CreateMap<CreateTaskRequest, Domain.Entities.Task>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.TaskTags, opt => opt.Ignore());

        CreateMap<UpdateTaskRequest, Domain.Entities.Task>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.IsCompleted, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.TaskTags, opt => opt.Ignore());

        CreateMap<Category, CategoryResponse>();
        CreateMap<Tag, TagResponse>();
        CreateMap<User, UserProfileResponse>();
    }
}
