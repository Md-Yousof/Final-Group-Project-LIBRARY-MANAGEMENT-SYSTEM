using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private IAuthor _author;
        public AuthorController(IAuthor author)
        {
            _author = author;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var author = await _author.GetAll();
            return Ok(author);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var author = await _author.Get(id);
            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Author author)
        {
            await _author.Post(author);
            return Ok(author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Author author)
        {
            await _author.Put(author);
            return Ok(author);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _author.Delete(id);
            return Ok(new { message = "Deleted successfully" });
        }

        [HttpGet, Route("SearchAuthor/{searchString}")]
        public IActionResult SearchByAuthorName(string searchString)
        {
            var author = _author.Search(searchString);
            return Ok(author);
        }

        [HttpGet, Route("GetActiveAuthor")]
        public IActionResult GetActiveAuthor()
        {
            var author = _author.GetActive();
            return Ok(author);
        }

        [HttpGet, Route("GetInactiveAuthors")]
        public IActionResult GetInactiveAuthor()
        {
            var author = _author.GetInactive();
            return Ok(author);
        }


    }
}
