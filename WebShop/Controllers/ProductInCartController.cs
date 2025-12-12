using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductInCartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductInCartController(ApplicationDbContext context) => _context = context;

        // dodavanje novog proizvoda u košaricu
        [HttpPost]
        public async Task<ActionResult<ProductInCart>> AddProductToCart(ProductInCart productInCart)
        {
            // što ako već imamo određeni poizvod u košarici?
            // projeriti dal ima taj proizvod u košarici već, ako da povećati amount, a s time i price
            
            _context.ProductInCarts.Add(productInCart);
            await _context.SaveChangesAsync();

            return Ok(productInCart); 
        }

        // dohvat svih proizvoda jedne košarice
        [HttpGet("cart/{cartId}")]
        public async Task<ActionResult<IEnumerable<ProductInCart>>> GetProductsInCart(int cartId)
        {
            var items = await _context.ProductInCarts
                                      .Where(p => p.CartId == cartId)
                                      .Include(p => p.Product)
                                      .ToListAsync();

            if (items == null || !items.Any())
                return NotFound($"No products found in CartId {cartId}");

            return Ok(items);
        }

        // proizvod u košarici - ažuiranje samo količine
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAmount(int id, [FromBody] int amount)
        {
            var item = await _context.ProductInCarts.FindAsync(id);
            if (item == null) return NotFound();

            item.Amount = amount;
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // micanje proizvoda iz košarice
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductFromCart(int id)
        {
            var item = await _context.ProductInCarts.FindAsync(id);
            if (item == null) return NotFound();

            _context.ProductInCarts.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
