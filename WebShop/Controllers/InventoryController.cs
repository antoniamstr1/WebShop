
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;
using WebShop.Entities;
namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public InventoryController(ApplicationDbContext context) => _context = context;


        // info da li određenog proizvoda ima u skladištu
        [HttpGet("quantity/{productId}")]
        public async Task<ActionResult<int>> GetInventoryQuantity(int productId)
        {
            var inventory = await _context.Inventories
                                          .FirstOrDefaultAsync(i => i.ProductId == productId);

            if (inventory == null)
                return NotFound($"No inventory found for ProductId {productId}");

            return Ok(inventory.InStockNumber);
        }

        // dohvat svog inventara - admin 

        // dodavanje novog inventara - admin 

        // mijenjanje inventara - admin

        // brisanje inventara - admin

    }

}