using WebShop.Data;
using WebShop.Models;
using Microsoft.EntityFrameworkCore;
using WebShop.Entities;
namespace WebShop.Services
{
    public interface IProductsInCartService
    {
        Task<ProductInCart> AddProductToCart(int cartId, int productId);
    }
}
namespace WebShop.Services
{

    public class ProductsInCartService : IProductsInCartService
    {
        private readonly ApplicationDbContext _context;

        public ProductsInCartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductInCart> AddProductToCart(int cartId, int productId)
        {
            var existingItem = await _context.ProductInCarts
             .FirstOrDefaultAsync(p =>
                 p.CartId == cartId &&
                 p.Product.Id == productId);

            if (existingItem != null)
            {
                existingItem.Amount += 1;
                await _context.SaveChangesAsync();
                return existingItem;
            }

            var product = await _context.Products.FindAsync(productId) ?? throw new Exception($"Product with id {productId} not found");
            var productInCart = new ProductInCart
            {
                CartId = cartId,
                Product = product,
                Amount = 1
            };

            _context.ProductInCarts.Add(productInCart);
            await _context.SaveChangesAsync();

            return productInCart;
        }
    }
}