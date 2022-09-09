using AutoMapper;
using Core;
using Infrastructure.DTOs;

namespace Infrastructure;

public class MappingProfiles: AutoMapper.Profile
{
    public MappingProfiles()
    {
        CreateMap<RegisterDto, AppUser>()
            .ForMember(x => x.CreationDate, y => y.AddTransform(t => DateTime.UtcNow));

        CreateMap<AppUser, Core.Profile>();
        
        CreateMap<Product, CartProduct>()
            .ForPath(d => d.Product.Id, o => o.MapFrom(x => x.Id))
            .ForPath(d => d.Product.Name, o => o.MapFrom(x => x.Name))
            .ForPath(d => d.Product.Description, o => o.MapFrom(x => x.Description))
            .ForPath(d => d.Product.Price, o => o.MapFrom(x => x.Price));
        
        CreateMap<CartProduct, ProductToCartDto>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.ProductId))
            .ForMember(d => d.Quantity, o => o.MapFrom(s => s.ProductQuantity))
            .ForMember(d => d.Price, o => o.MapFrom(s => s.Product.Price));
    }

}
