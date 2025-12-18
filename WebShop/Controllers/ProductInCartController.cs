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
        //TODO: da se čita amount, ne da se palje
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

        [HttpHead("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }


    }
}
