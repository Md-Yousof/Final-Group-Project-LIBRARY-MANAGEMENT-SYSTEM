using LibraryAPI_R53_A.Core.Entities;
using LibraryAPI_R53_A.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI_R53_A.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoice _inv;

        public InvoiceController(IInvoice inv)
        {
            _inv = inv;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var inv = await _inv.GetAll();
            return Ok(inv);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult>GetOne(int id)
        {
            var inv = await _inv.Get(id);
            return Ok(inv);
        }

        [HttpPost]
        public async Task<ActionResult<Invoice>>Post(Invoice inv)
        {
            inv.TransactionDate = DateTime.Now;
            inv.BorrowId = null;
            inv.Fine = null;
            inv.MiscellaneousFines = null;
            inv.Refund = null;
            await _inv.Post(inv);
            return Ok(inv);
        }

    }
}
