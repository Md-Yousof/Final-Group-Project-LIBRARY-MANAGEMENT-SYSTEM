using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Interfaces;
using LibraryAPI_R53_A.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCopyController : ControllerBase
    {
        private IBookCopy _copy;
        public BookCopyController(IBookCopy copy)
        {
            _copy = copy;
        }
        [HttpGet]
        public async Task<IActionResult> GetBookCopies()
        {
            var copy = await _copy.GetAll();
            return Ok(copy);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var copy = await _copy.Get(id);
            return Ok(copy);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookCopy model)
        {
            await _copy.Post(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(BookCopy copy)
        {
            await _copy.Put(copy);
            return Ok(copy);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _copy.Delete(id);
            return Ok(new { message = "Deleted successfully" });
        }

        [HttpGet, Route("SearchBookCopy/{searchString}")]
        public IActionResult SearchByBookName(string searchString)
        {
            var copy = _copy.Search(searchString);
            return Ok(copy);
        }

        [HttpGet, Route("GetActiveBookCopy")]
        public IActionResult GetActiveBookCopy()
        {
            var copy = _copy.GetActive();
            return Ok(copy);
        }

        [HttpGet, Route("GetInactiveBookCopies")]
        public IActionResult GetInactiveBookCopy()
        {
            var copy = _copy.GetInactive();
            return Ok(copy);
        }

        [HttpGet, Route("GoodCondition")]
        public IActionResult GoodCondition()
        {
            var copy = _copy.GoodCondition();
            return Ok(copy);
        }

        
        [HttpGet, Route("GetToRepairCopies")]
        public IActionResult GetToRepairCopies()
        {
            var copy = _copy.GotToRepair();
            return Ok(copy);
        }

        [HttpGet, Route("GetDamagedCopies")]
        public IActionResult GetDamagedCopies()
        {
            var copy = _copy.GetDamaged();
            return Ok(copy);
        }   

    }
}
