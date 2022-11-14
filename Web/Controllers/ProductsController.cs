using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core;
using Data;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

// [Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public ProductsController(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    //test
    [HttpGet]
    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        //project to productDto with string array instead of photos and main photo 
        return await _context.Products
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ProductDto?> GetProduct(Guid id)
    {
        return _mapper.Map<Product, ProductDto>(await _context.Products.FindAsync(id)??null!); 
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var prod = await _context.Products.FindAsync(id);
        if(prod != null)
        {
            _context.Products.Remove(prod);
            var res = await _context.SaveChangesAsync() > 0;
            return res ?
            Ok() : BadRequest("Could not persist changes in the database");
        }
        return BadRequest("Invalid product ID");
    }

    [Authorize(Roles = "Administrator")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateProduct([FromBody]ProductDto prod, Guid id)
    {
        var change = await _context.Products.Include(x => x.Photos).FirstAsync(x => x.Id == id);
        if(change is null)
        {
            return BadRequest("Product does not exist");
        }
        _mapper.Map(prod, change);

        var res =  await _context.SaveChangesAsync(); 

        return res > 0 ? Ok() : 
        BadRequest("Could not persist changes in the database");
    }

    [Authorize(Roles = "Administrator")]
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]ProductDto prod)
    {
        var product = _mapper.Map<ProductDto, Product>(prod);

        _context.Products.Add(product);

        var res = await _context.SaveChangesAsync();
        if(res <= 0)
        {
            BadRequest("Could not persist changes");
        }
        prod.Id = product.Id;

        return Ok(prod);
    }
}
