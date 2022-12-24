using AutoMapper;
using DotNet_API_Example.Models;
using DotNet_API_Example.Models.Dto;

namespace DotNet_API_Example
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Blog, BlogDTO>();
            CreateMap<BlogDTO, Blog>();

            CreateMap<Blog, BlogCreateDTO>().ReverseMap();
            CreateMap<Blog, BlogUpdateDTO>().ReverseMap();
            

            CreateMap<BlogNumber, BlogNumberDTO>().ReverseMap();
            CreateMap<BlogNumber, BlogNumberCreateDTO>().ReverseMap();
            CreateMap<BlogNumber, BlogNumberUpdateDTO>().ReverseMap();

        }
    }
}
