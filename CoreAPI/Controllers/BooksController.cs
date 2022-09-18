using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoreAPI.Models;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Collections.Generic;
using System.Net;

namespace CoreAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BooksController : Controller
    {
        private readonly APIContext dbContext;

        public BooksController(APIContext context)
        {
            dbContext = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await dbContext.Books.ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id == null || dbContext.Books == null)
                return NotFound();

            var book = await dbContext.Books
                .FirstOrDefaultAsync(p => p.Id == id);
            if (book == null)
                return NotFound();
            
            var links = dbContext.Links.ToList().FindAll(p => p.BookId == id);
            List<Author> authors = new List<Author>(); 
            foreach(Author author in dbContext.Authors.ToList())
            {
                if (links.FirstOrDefault(p => p.AuthorId == author.Id) != null)
                    authors.Add(author);
            }
            var result = new {book, authors};

            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Post(RequestBook reqbook)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Book book = new Book
            {
                Title = reqbook.Title,
                Pages = reqbook.Pages
            };

            await dbContext.AddAsync(book);
            await dbContext.SaveChangesAsync();

            return Ok(book);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.Books == null)
                return NotFound();

            var book = await dbContext.Books
                .FirstOrDefaultAsync(p => p.Id == id);
            if (book == null)
                return NotFound();

            dbContext.Remove(book);
            await dbContext.SaveChangesAsync();
            return Ok(book);
        }
        [HttpPut]
        public async Task<IActionResult> Put(int? id, RequestBook reqbook)
        {
            if (id == null || dbContext.Books == null)
                return NotFound();
            var book = await dbContext.Books
                .FirstOrDefaultAsync(p => p.Id == id);
            if (book == null)
                return NotFound();

            book.Title = reqbook.Title;
            book.Pages = reqbook.Pages;
            await dbContext.SaveChangesAsync();

            return Ok(book);
        }
        [HttpPost("AddAuthor")]
        public async Task<IActionResult> AddAuthor(int? bookid, RequestAuthor reqauthor)
        {
            if (bookid == null)
                return NotFound();
            var author = await dbContext.Authors
                .FirstOrDefaultAsync(p => p.Name == reqauthor.Name);
            if (author == null)
            {
                author = new Author
                {
                    Name = reqauthor.Name
                };
                await dbContext.AddAsync(author);
                await dbContext.SaveChangesAsync();
            }
            if (dbContext.Links.Any(p => p.BookId == bookid && p.AuthorId == author.Id))
            {
                return BadRequest("Author already added for this book");
            }
            Link link = new Link
            {
                AuthorId = author.Id,
                BookId = (int)bookid
            };
            await dbContext.AddAsync(link);
            await dbContext.SaveChangesAsync();

            return Ok("Author added successfully");
        }
        [HttpDelete("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor(int? bookid, RequestAuthor reqauthor)
        {
            if (bookid == null)
                return NotFound();

            var author = await dbContext.Authors
                .FirstOrDefaultAsync(p => p.Name == reqauthor.Name);
            if (author == null)
            {
                return BadRequest("Author is not found");
            }

            var link = await dbContext.Links
                .FirstOrDefaultAsync(p => p.BookId == bookid && p.AuthorId == author.Id);
            if (link == null)
            {
                return BadRequest("Author is not linked to this book");
            }

            dbContext.Remove(link);
            await dbContext.SaveChangesAsync();

            return Ok("Author removed successfully.");
        }
    }
}
