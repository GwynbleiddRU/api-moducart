using AutoMapper;
using ProductService.API.DTOs;
using ProductService.API.Models;

namespace ProductService.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product mappings
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => 
                    src.Category != null ? src.Category.Name : string.Empty));

            CreateMap<ProductDto, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            // Category mappings
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
        }
    }
}