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

        CreateMap<Product, ProductDto>()
            .ForMember(d => d.Photos, o => o.MapFrom(s => s.Photos.Select(x => x.Url)));
        CreateMap<ProductDto, Product>()
            .ForMember(d => d.CartProducts, o=> o.Ignore())
            .ForMember(d => d.Id, o=> o.Ignore())
            .ForMember(d => d.Photos, o=> o.MapFrom(s => s.Photos!.Select(x => 
                new Photo
                {
                    Url = x
                })));
        
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

        CreateMap<CartProduct,ShoppingCartItem>()
            .ForMember(x => x.Quantity, o => o.MapFrom(s => s.ProductQuantity));

        CreateMap<ShoppingCartItem, CartProduct>()
            .ForMember(x => x.ProductId, o => o.MapFrom(x => x.Product.Id))
            .ForMember(x => x.Product, o => o.Ignore())
            .ForMember(x => x.ProductQuantity, o => o.MapFrom(s => s.Quantity));
        
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

        CreateMap<CountableProduct, ShoppingCartItem>();

        CreateMap<CountableProduct, ProductDto>()
            .ForMember(x => x.Description, o=>o.MapFrom(s => s.Product.Description))
            .ForMember(x => x.Name, o=>o.MapFrom(s => s.Product.Name))
            .ForMember(x => x.Price, o=>o.MapFrom(s => s.Product.Price))
            .ForMember(x => x.Photos, o=>o.MapFrom(s => s.Product.Photos.Select(x => x.Url)));
        
        CreateMap<AppUser, ClientDto>()
            .ForMember(d => d.CreatedAt, o=>o.MapFrom(s => s.CreationDate))
            .ForMember(d => d.Username, o=>o.MapFrom(s => s.UserName))
            .ForPath(d => d.MoneySpent, o=>o.MapFrom(s => 
                s.Transactions.Aggregate(0M,(a,b) => a + b.Price)
            ))
            .ForMember(d => d.Privileges, o=>o.Ignore());
    }

}
