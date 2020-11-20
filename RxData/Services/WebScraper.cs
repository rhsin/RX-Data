using AngleSharp;
using RxData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RxData.Services
{
    public interface IWebScraper
    {
        public Task<IEnumerable<RxPrice>> GetRxPrices(string medication);
    }

    public class WebScraper : IWebScraper
    {
        private readonly IBrowsingContext _context;

        public WebScraper()
        {
            _context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        }

        public async Task<IEnumerable<RxPrice>> GetRxPrices(string medication)
        {
            var source = $@"https://www.singlecare.com/prescription/{medication}";
            var document = await _context.OpenAsync(source);
            var elements = document.QuerySelectorAll("div.pharmacy-item");
            var rxPrices = new List<RxPrice>();

            foreach (var e in elements)
            {
                float price; 
                float.TryParse(e.QuerySelector("p.pharmacy-item__price")?.TextContent.Trim('$', ' '), out price);

                rxPrices.Add(new RxPrice
                {
                    Name = e.QuerySelector("img")?.GetAttribute("data-name"),
                    Price = price
                });
            }

            return rxPrices;
        }
    }
}
