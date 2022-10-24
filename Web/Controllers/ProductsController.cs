using Core;
using Data;
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
    public ProductsController(DataContext context)
    {
        _context = context;
    }

    //test
    [HttpGet]
    public async Task<List<Product>> GetProducts()
    {
        return await _context.Products.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<Product?> GetProduct(Guid id)
    {
        return await _context.Products.FindAsync(id); 
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
}
