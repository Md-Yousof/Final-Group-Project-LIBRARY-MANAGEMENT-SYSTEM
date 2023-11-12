using LibraryAPI_R53_A.Core.Domain;
using LibraryAPI_R53_A.Core.Repositories;
using LibraryAPI_R53_A.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private IRepository<Subcategory> _repo;
        public SubcategoryController(IRepository<Subcategory> repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetSubcategories()
        {
            var subcategory = await _repo.GetAll();
            return Ok(subcategory);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var subcategory = await _repo.Get(id);
            return Ok(subcategory);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Subcategory model)
        {
            await _repo.Post(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(Subcategory subcategory)
        {
            await _repo.Put(subcategory);
            return Ok(subcategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            await _repo.Delete(id);
            return Ok(new { message = "Deleted successfully" });
        }

        [HttpGet, Route("SearchSubcategory/{searchString}")]
        public IActionResult SearchBySubcategoryName(string searchString)
        {
            var subcategory = _repo.Search(searchString);
            return Ok(subcategory);
        }

        //[HttpGet, Route("GetActiveSubcategory")]
        //public IActionResult GetActiveSubcategory()
        //{
        //    var subcategory = _repo.GetActive();
        //    return Ok(subcategory);
        //}

        //[HttpGet, Route("GetInactiveSubcategorys")]
        //public IActionResult GetInactiveSubcategory()
        //{
        //    var subcategory = _repo.GetInactive();
        //    return Ok(subcategory);
        //}

    }
}
