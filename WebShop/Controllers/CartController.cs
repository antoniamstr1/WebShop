using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context) => _context = context;

        // dohvat trenutne košarice kupca
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<Cart>> GetCartForCustomer(string customerId)
        {
            var cart = await _context.Carts
                                     .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (cart == null)
                return NotFound($"No cart found for CustomerId {customerId}");

            return Ok(cart);
        }

        //košarica mijenja status iz terminated nakon 20 minuta ako se nije u međuvremenu platilo -> status je stavljen na paid

        // kreiranje nove košarice - paziti da samo jedna može biti u istom trenutku
        [HttpPost("customer/{customerId}")]
        public async Task<ActionResult<Cart>> CreateCartForCustomer(string customerId)
        {

            var existingCart = await _context.Carts
                                             .FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.Terminated);

            if (existingCart != null)
                return BadRequest("Active cart from customer.");

            var cart = new Cart
            {
                CustomerId = customerId,
                StartTime = DateTime.UtcNow,
                PaymentConfirmed = false,
                Terminated = false
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartForCustomer), new { customerId = customerId }, cart);
        }


    }

}