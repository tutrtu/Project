using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly QandAContext _context;
        public CategoriesController(QandAContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult<IEnumerable<Category>> Get()
        {
            return Ok(_context.Categories.ToList());
        }

        [HttpPost]
        public ActionResult<Category> Post(NewCategoryDto newCategoryDto)
        {
            if (newCategoryDto == null || string.IsNullOrWhiteSpace(newCategoryDto.CategoryName))
            {
                return BadRequest("Category name cannot be null or empty.");
            }

            var category = new Category
            {
                CategoryName = newCategoryDto.CategoryName
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = category.CategoryId }, category);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
