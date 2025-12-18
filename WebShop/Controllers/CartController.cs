using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;
using WebShop.Services;


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
        [HttpGet("customer")]
        [Authorize]
        public async Task<ActionResult<Cart>> GetCartForCustomer()
        {
            var nameIdClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            string? customerId = nameIdClaim?.Value;

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized("User not found in token.");

            var cart = await _context.Carts
                 .Include(c => c.Customer)
                 .FirstOrDefaultAsync(c =>
                     (c.Customer != null && c.Customer.Id.ToString() == customerId && !c.Terminated)
                     || (c.AnonymousCustomer != null && c.AnonymousCustomer == customerId && !c.Terminated)
                 );
            if (cart == null)
                return Ok(new
                {
                    Cart = (Cart?)null,
                    ProductsInCart = new List<ProductInCart>()
                });
            var productsInCart = await _context.ProductInCarts
                .Include(pic => pic.Product)
                .Where(pic => pic.CartId == cart.Id)
                .ToListAsync();

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
        [Authorize]
        public async Task<IActionResult> AddToCart([FromBody] AddProductRequest request)
        {
            var nameIdClaim = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
            string? customerId = nameIdClaim?.Value;

            if (string.IsNullOrEmpty(customerId))
                return Unauthorized("User not found in token.");

            int? cartId = request.CartId;
            int productId = request.ProductId;

            var existingCart = await _context.Carts
                                             .FirstOrDefaultAsync(c => c.Id == cartId && !c.Terminated);

            Cart cart;

            // ako već nema košarice- kreiram ju
            if (existingCart == null)
            {
                Customer? currentUser = null;
                string? anonymousCurrentUser = null;

                // ako imamo customerId 
                if (!string.IsNullOrEmpty(customerId))
                {
                    // customer je prijavljen
                    currentUser = await _context.Customers
                                    .FirstOrDefaultAsync(c => c.Id.ToString() == customerId);
                    if (currentUser == null)
                        anonymousCurrentUser = customerId;
                }

                // kreiranje košarice
                cart = new Cart
                {
                    Customer = currentUser,
                    AnonymousCustomer = anonymousCurrentUser,
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

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
        }


    }

}