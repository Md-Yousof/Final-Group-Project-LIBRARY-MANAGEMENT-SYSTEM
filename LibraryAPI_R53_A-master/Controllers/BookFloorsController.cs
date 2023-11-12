using AutoMapper;
using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;
using LibraryAPI_R53_A.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookFloorsController : ControllerBase
    {
        private IRepository<BookFloor> _repo;
        private readonly IMapper _mapper;
        public BookFloorsController(IRepository<BookFloor> Repo, IMapper map)
        {
            _repo = Repo;
            _mapper = map;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookFloors()
        {
            var BookFloor = await _repo.GetAll();
            return Ok(BookFloor);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var BookFloor = await _repo.Get(id);
            return Ok(BookFloor);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookFloor model)
        {
            await _repo.Post(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(BookFloor bookFloor)
        {
            // bookFloor = await _repo.Get(id);
            //if (bookFloor == null)
            //{
            //    return NotFound();
            //}
            await _repo.Put(bookFloor);
            return Ok(bookFloor);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _repo.Delete(id);
            return Ok(new { message = "Deleted successfully" });
        }

    }
}
