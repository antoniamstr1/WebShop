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


    }
}
