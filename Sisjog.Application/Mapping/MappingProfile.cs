using AutoMapper;
using Sisjog.Application.DTOs;
using Sisjog.Domain.Entities;


namespace Sisjog.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<VideoGameConsole, ConsoleDto>().ReverseMap();

        }
    }
}
