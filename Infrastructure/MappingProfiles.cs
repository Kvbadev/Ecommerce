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
        
        CreateMap<ShoppingCart, ShoppingCartDto>()
            .ForMember(d => d.Sum, o => o.MapFrom(s => s.FinalPrice))
            .ForMember(d => d.Items, o => o.MapFrom(s =>  s.CartProducts));

        CreateMap<ShoppingCartDto, ShoppingCart>()
            .ForMember(d => d.FinalPrice, o => o.MapFrom(s => s.Sum))
            .ForMember(d => d.CartProducts, o => o.MapFrom(s => s.Items))
            .ForMember(d => d.Count, o => o.MapFrom(s => s.Count));
        
        CreateMap<CartProduct, ProductSimplified>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.ProductId))
            .ForMember(d => d.Quantity, o => o.MapFrom(s => s.ProductQuantity));

        CreateMap<ProductSimplified, CartProduct>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.ProductQuantity, o => o.MapFrom(s => s.Quantity));
        
        CreateMap<Core.Profile, AppUser>()
            .ForMember(d => d.UserName, o => o.Ignore());
        
        CreateMap<CartProduct, CountableProduct>()
            .ForMember(d => d.Quantity, o => o.MapFrom(s => s.ProductQuantity));
        
        CreateMap<CountableProduct, ProductSimplified>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Product.Id))
            .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity));

        CreateMap<Transaction, TransactionDto>()
            .ForMember(d => d.Products, o => o.MapFrom(s => s.Products))
            .ForMember(d => d.Success, o=>o.MapFrom(s => !s.Failure));
    }

}
