using Microsoft.EntityFrameworkCore;
using RxData.Data;
using RxData.Models;
using RxData.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxData.Repositories
{
    public interface IRxPriceRepository
    {
        public Task<IEnumerable<RxPrice>> GetAll();
        public Task Create(RxPrice rxPrice);
        public Task Update(RxPrice rxPrice);
        public Task Delete(int id);
        public Task SeedRxPrices(string medication);
        public bool RxPricesSeeded(string medication);
    }

    public class RxPriceRepository : IRxPriceRepository
    {
        private readonly RxContext _context;
        private readonly IWebScraper _webScraper;

        public RxPriceRepository(RxContext context, IWebScraper webScraper)
        {
            _context = context;
            _webScraper = webScraper;
        }

        public async Task<IEnumerable<RxPrice>> GetAll()
        {
            return await _context.RxPrices.ToListAsync();
        }

        public async Task Create(RxPrice rxPrice)
        {
            _context.RxPrices.Add(rxPrice);
            await _context.SaveChangesAsync();
        }

        public async Task Update(RxPrice rxPrice)
        {
            _context.Entry(rxPrice).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var rxPrice = await _context.RxPrices.FindAsync(id);

            _context.RxPrices.Remove(rxPrice);
            await _context.SaveChangesAsync();
        }

        public async Task SeedRxPrices(string medication)
        {
            var rxPrices = await _webScraper.GetRxPrices(medication);
            var rxPricesCanada = await _webScraper.GetRxPricesCanada(medication);

            _context.RxPrices.AddRange(rxPrices);
            _context.RxPrices.AddRange(rxPricesCanada);
            await _context.SaveChangesAsync();
        }

        public bool RxPricesSeeded(string medication)
        {
            return _context.RxPrices.Any(rp => rp.Name == medication.ToLower());
        }
    }
}
