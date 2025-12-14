using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;
using WebShop.Services;
using WebShop.Entities;
namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductsInCartService _productsInCartService;

        public CartController(ApplicationDbContext context, IProductsInCartService productsInCartService)
        {
            _context = context;
            _productsInCartService = productsInCartService;
        }



        // dohvat trenutne košarice kupca
        [HttpGet("customer/{cartId}")]
        public async Task<ActionResult<Cart>> GetCartForCustomer(int cartId)
        {
            var cart = await _context.Carts
                                     .FirstOrDefaultAsync(c => c.Id == cartId);
            var productsInCart = await _context.ProductInCarts
                .Include(pic => pic.Product)
                .Where(pic => pic.CartId == cartId)
                .ToListAsync();

            if (cart == null)
                return NotFound($"No cart found for cartId {cartId}");

            return Ok(new
            {
                Cart = cart,
                ProductsInCart = productsInCart
            });
        }

        //košarica mijenja status iz terminated nakon 20 minuta ako se nije u međuvremenu platilo -> status je stavljen na paid

        // dodavanje proizvoda u košaricu -> kreiranje nove košarice - paziti da samo jedna može biti u istom trenutku
        public class AddProductRequest
        {
            public int? CartId { get; set; }
            public string? CustomerId { get; set; }
            public int ProductId { get; set; }
        }

        [HttpPost("add")]
        /* [HttpPost("add/{customerId}/{productId}")] */

        public async Task<IActionResult> AddToCart([FromBody] AddProductRequest request)
        {
            string? customerId = request.CustomerId;
            int? cartId = request.CartId;
            int productId = request.ProductId;

            var existingCart = await _context.Carts
                                             .FirstOrDefaultAsync(c => c.Id == cartId && !c.Terminated);

            Cart cart;

            // ako već nema košarice- kreiram ju
            if (existingCart == null)
            {
                // defaultno - ako customer nije prijavljen
                Customer? currentUser = null;

                // ako imamo customerId - customer je prijavljen
                if (customerId != null)
                {
                    currentUser = await _context.Customers
                                                .FirstOrDefaultAsync(c => c.Id.ToString() == customerId);
                    if (currentUser == null)

                        return BadRequest("Cannot find user.");
                }

                // kreiranje košarice
                cart = new Cart
                {
                    Customer = currentUser,
                    StartTime = DateTime.UtcNow,
                    PaymentConfirmed = false,
                    Terminated = false,

                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            else
            {
                cart = existingCart;
            }

            // dodajem proizvod
            try
            {
                var productInCart = await _productsInCartService.AddProductToCart(cart.Id, productId);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to add product to cart.");
            }

            await _context.SaveChangesAsync();

            // povećavam price košarice
            var productsInCart = await _context.ProductInCarts
                                   .Include(pic => pic.Product)
                                   .Where(pic => pic.CartId == cart.Id)
                                   .ToListAsync();
            decimal totalPrice = productsInCart.Sum(pic => pic.Amount * pic.Product.Price);

            cart.Price = Convert.ToDecimal(totalPrice);

            await _context.SaveChangesAsync();


            return Ok(cart.Id);
        }


    }

}