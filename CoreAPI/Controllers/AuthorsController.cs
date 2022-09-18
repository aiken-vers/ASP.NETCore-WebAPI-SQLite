using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreAPI.Models;
using static System.Reflection.Metadata.BlobBuilder;
using System.ComponentModel;

namespace CoreAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthorsController : Controller
    {
        private readonly APIContext dbContext;

        public AuthorsController(APIContext context)
        {
            dbContext = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await dbContext.Authors.ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null || dbContext.Authors == null)
                return NotFound();

            var author = await dbContext.Authors
                .FirstOrDefaultAsync(p => p.Id == id);
            if (author == null)
                return NotFound();

            return Ok(author);
        }        
        [HttpPost]
        public async Task<IActionResult> Post(RequestAuthor reqauthor)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Author author = new Author
            {
                Name = reqauthor.Name
            };

            await dbContext.AddAsync(author);
            await dbContext.SaveChangesAsync();
            
            return Ok(author);
        }        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.Authors == null)
                return NotFound();

            var author = await dbContext.Authors
                .FirstOrDefaultAsync(p => p.Id == id);
            if (author == null)
                return NotFound();

            dbContext.Remove(author);
            await dbContext.SaveChangesAsync();
            return Ok(author);
        }
        [HttpPut]
        public async Task<IActionResult> Put(int? id, RequestAuthor reqauthor)
        {
            if (id == null || dbContext.Authors == null)
                return NotFound();
            var author = await dbContext.Authors
                .FirstOrDefaultAsync(p => p.Id == id);
            if (author == null)
                return NotFound();

            author.Name = reqauthor.Name;
            await dbContext.SaveChangesAsync();

            return Ok(author);
        }
    }
}
