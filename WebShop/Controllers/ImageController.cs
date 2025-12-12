using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ImageController(ApplicationDbContext context) => _context = context;

        // dohvat slike za specifičan proizvod
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Image>>> GetImagesByProduct(int productId)
        {
            var images = await _context.Images
                                       .Where(i => i.ProductId == productId)
                                       .ToListAsync();

            if (!images.Any())
                return NotFound($"No images found for ProductId {productId}");

            return Ok(images);
        }

        // dodavanje slike za poizvod
        [HttpPost]
        public async Task<ActionResult<Image>> AddImage(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return Ok(image);
        }

        // brisanje slike za proizvod
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null) return NotFound();

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }

}