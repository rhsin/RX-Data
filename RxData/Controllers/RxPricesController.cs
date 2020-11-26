using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RxData.Models;
using RxData.Repositories;
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
        private readonly IRxPriceRepository _rxPriceRepository;
        private readonly IPriceCalculator _priceCalculator;
        private readonly IWebScraper _webScraper;

        public RxPricesController(IRxPriceRepository rxPriceRepository, IPriceCalculator priceCalculator,
            IWebScraper webScraper)
        {
            _rxPriceRepository = rxPriceRepository;
            _priceCalculator = priceCalculator;
            _webScraper = webScraper;
        }

        // GET: api/RxPrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RxPrice>>> GetRxPrices()
        {
            return Ok(await _rxPriceRepository.GetAll());
        }

        // GET: api/RxPrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RxPrice>> GetRxPrice(int id)
        {
            var rxPrices = await _rxPriceRepository.GetAll();
            var rxPrice = rxPrices.FirstOrDefault(rp => rp.Id == id);

            if (rxPrice == null)
            {
                return NotFound();
            }

            return rxPrice;
        }

        // GET: api/RxPrices/Fetch/Baclofen
        [HttpGet("Fetch/{medication}")]
        public async Task<ActionResult<IEnumerable<RxPrice>>> FetchRxPrices(string medication)
        {
            return Ok(await _webScraper.GetRxPrices(medication));
        }

        // GET: api/RxPrices/Fetch/Canada/Baclofen
        [HttpGet("Fetch/Canada/{medication}")]
        public async Task<ActionResult<IEnumerable<RxPrice>>> FetchRxPricesCanada(string medication)
        {
            return Ok(await _webScraper.GetRxPricesCanada(medication));
        }

        // GET: api/RxPrices/Fetch/Alt/Canada/Baclofen
        [HttpGet("Fetch/Alt/Canada/{medication}")]
        public async Task<ActionResult<IEnumerable<RxPrice>>> FetchRxPricesCanadaAlt(string medication)
        {
            return Ok(await _webScraper.GetRxPricesCanadaAlt(medication));
        }

        // GET: api/RxPrices/Find/Baclofen
        [HttpGet("Find/{name}")]
        public async Task<ActionResult<RxPriceDTO>> FindRxPrices(string name)
        {
            return Ok(await _rxPriceRepository.FindAll(name));
        }

        // GET: api/RxPrices/Values/Baclofen
        [HttpGet("Values/{name}")]
        public async Task<ActionResult<RxPriceDTO>> FindValues(string name, string column, string value)
        {
            return Ok(await _rxPriceRepository.FindBy(name, column, value));
        }

        // GET: api/RxPrices/Medication
        [HttpGet("Medication")]
        public async Task<ActionResult<RxPriceDTO>> FindMedication(string name, string location, float price)
        {
            return Ok(await _rxPriceRepository.FindMedication(name, location, price));
        }

        // GET: api/RxPrices/Price/Mg
        [HttpGet("Price/Mg")]
        public async Task<ActionResult<IEnumerable<RxPrice>>> GetRxPricesPerMg(string name)
        {
            var rxPrices = await _rxPriceRepository.GetMedication(name);

            return Ok(_priceCalculator.PricePerMg(rxPrices));
        }

        // POST: api/RxPrices/Seeder/Baclofen
        [Authorize]
        [HttpPost("Seeder/{medication}")]
        public async Task<ActionResult<string>> SeedRxPrices(string medication)
        {
            if (_rxPriceRepository.RxPricesSeeded(medication))
            {
                return BadRequest($"RxPrices Already Seeded: {medication}!");
            }

            await _rxPriceRepository.SeedRxPrices(medication);

            return Ok($"RxPrices Seeded Successfully: {medication}!");
        }

        // PUT: api/RxPrices/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRxPrice(int id, RxPrice rxPrice)
        {
            if (id != rxPrice.Id)
            {
                return BadRequest();
            }

            await _rxPriceRepository.Update(rxPrice);

            return NoContent();
        }

        // POST: api/RxPrices
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<RxPrice>> PostRxPrice(RxPrice rxPrice)
        {
            await _rxPriceRepository.Create(rxPrice);

            return CreatedAtAction("GetRxPrice", new { id = rxPrice.Id }, rxPrice);
        }

        // DELETE: api/RxPrices/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RxPrice>> DeleteRxPrice(int id)
        {
            await _rxPriceRepository.Delete(id);

            return NoContent();
        }
    }
}
