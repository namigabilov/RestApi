using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DataAccessLayer;
using WebApplication4.Entities;

namespace WebApplication4.Controllers
{
    [Route("api/mehsullar")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _context.Products.AnyAsync(p=>p.Tittle.ToLower() == product.Tittle.Trim().ToLowerInvariant()))
            {
                return Conflict($"{product.Tittle} Tittle Adi altinda Mehsul Movcuddur !");
            }
            product.CreatedBy = "System";
            product.CreatedAt = DateTime.UtcNow.AddHours(4);

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return StatusCode(201,$" {product.Id} - Id , Created!");
        }
        [HttpGet]
        [Route("{id?}")]
        public async Task<IActionResult> GetById(int? id)
        {
            if (id == null) return BadRequest("Id Is Null");

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound("Id Is Incorrect");

            return Ok(product); 
        }
        [HttpGet]
        public   async Task<IActionResult> GetByList()
        {
            return Ok(await _context.Products.ToListAsync());
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromForm]Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Product dbProduct = await _context.Products.FirstOrDefaultAsync(p=>p.Id== product.Id);

            if (dbProduct == null) return NotFound();

            if (await _context.Products.AnyAsync(c=>c.Tittle.ToLower() == product.Tittle.Trim().ToLowerInvariant() && c.Id != product.Id))
            {
                return Conflict($"{product.Tittle} Is Already Taken Use Another One");
            }
            dbProduct.Tittle = product.Tittle;
            dbProduct.Price = product.Price;
            dbProduct.Count = product.Count;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete]
        [Route("{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest("Id Is Null");

            Product product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound("Id Is Incorrect");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
