using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RxData.Data;
using RxData.Models;
using RxData.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RxPricesController : ControllerBase
    {
        private readonly RxContext _context;
        private readonly IWebScraper _webScraper;

        public RxPricesController(RxContext context, IWebScraper webScraper)
        {
            _context = context;
            _webScraper = webScraper;
        }

        // GET: api/RxPrices/Fetch/Baclofen
        [HttpGet("Fetch/{medication}")]
        public async Task<ActionResult<IEnumerable<RxPrice>>> FetchRxPrices(string medication)
        {
            return Ok(await _webScraper.GetRxPrices(medication));
        }

        // GET: api/RxPrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RxPrice>>> GetRxPrices()
        {
            return await _context.RxPrices.ToListAsync();
        }

        // GET: api/RxPrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RxPrice>> GetRxPrice(int id)
        {
            var rxPrice = await _context.RxPrices.FindAsync(id);

            if (rxPrice == null)
            {
                return NotFound();
            }

            return rxPrice;
        }

        // PUT: api/RxPrices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRxPrice(int id, RxPrice rxPrice)
        {
            if (id != rxPrice.Id)
            {
                return BadRequest();
            }

            _context.Entry(rxPrice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RxPriceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RxPrices
        [HttpPost]
        public async Task<ActionResult<RxPrice>> PostRxPrice(RxPrice rxPrice)
        {
            _context.RxPrices.Add(rxPrice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRxPrice", new { id = rxPrice.Id }, rxPrice);
        }

        // DELETE: api/RxPrices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RxPrice>> DeleteRxPrice(int id)
        {
            var rxPrice = await _context.RxPrices.FindAsync(id);
            if (rxPrice == null)
            {
                return NotFound();
            }

            _context.RxPrices.Remove(rxPrice);
            await _context.SaveChangesAsync();

            return rxPrice;
        }

        private bool RxPriceExists(int id)
        {
            return _context.RxPrices.Any(e => e.Id == id);
        }
    }
}
