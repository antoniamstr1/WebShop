using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShop.Data;
using WebShop.Models;
using WebShop.Entities;
namespace WebShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AddressController(ApplicationDbContext context) => _context = context;

        //dodavanje nove adrese
        [HttpPost]
        public async Task<ActionResult<Address>> AddAddress([FromBody] Address address)
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAddressById), new { id = address.Id }, address);
        }

        // dohvat adrese određenog usera
        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddressById(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address == null) return NotFound();
            return Ok(address);
        }

        // ažuriranje adrese
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] Address address)
        {
            if (id != address.Id)
                return BadRequest("ID mismatch.");

            _context.Entry(address).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
