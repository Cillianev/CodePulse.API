using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using СodePulse.API.Data;
using СodePulse.API.Models.Domain;
using СodePulse.API.Models.DTO;

namespace СodePulse.API.Controllers
{
    // https://localhost:xxxx/api/categories
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public CategoriesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            // Retrieve categories from the database
            var categories = await dbContext.Categories
                .Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle,
                })
                .ToListAsync();

            return Ok();
        }

        //
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            //Map DTO to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };
            
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();


            // Domain model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok();

        }
    }
}
