using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShelfController : ControllerBase
    {
        private IRepository<Shelf> _repo;

        public ShelfController(IRepository<Shelf> repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> GetShelves()
        {
            var shelf = await _repo.GetAll();
            return Ok(shelf);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var shelf = await _repo.Get(id);
            return Ok(shelf);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Shelf model)
        {
            await _repo.Post(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Shelf shelf)
        {
            await _repo.Put(shelf);
            return Ok(shelf);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _repo.Delete(id);
            return Ok(new { message = "Deleted successfully" });
        }

    }
}

    

