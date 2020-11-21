using AngleSharp;
using RxData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxData.Services
{
    public interface IWebScraper
    {
        public Task<IEnumerable<RxPrice>> GetRxPrices(string medication);
        public Task<IEnumerable<RxPrice>> GetRxPricesCanada(string medication);
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
            var source = $"https://www.singlecare.com/prescription/{medication}";
            var document = await _context.OpenAsync(source);
            var elements = document.QuerySelectorAll("div.pharmacy-item");

            var details = document.QuerySelector("span.filter-amt--qty")?.TextContent
                .Split("Tablet,");

            int quantity;
            int.TryParse(details[0].Trim(), out quantity);

            int dose;
            int.TryParse(details[1].Trim().Replace("mg", ""), out dose);

            var rxPrices = new List<RxPrice>();

            foreach (var e in elements)
            {
                float price;
                float.TryParse(e.QuerySelector("p.pharmacy-item__price")?.TextContent
                    .Trim('$', ' '), out price);

                if (price > 0)
                {
                    rxPrices.Add(new RxPrice
                    {
                        Name = medication.ToLower(),
                        Quantity = quantity,
                        Dose = dose,
                        Price = price,
                        Location = e.QuerySelector("img")?.GetAttribute("data-name"),
                        VendorId = 1
                    });
                }
            }

            return rxPrices;
        }

        public async Task<IEnumerable<RxPrice>> GetRxPricesCanada(string medication)
        {
            var source = $"https://canadarx24h.com/catalog/view?slug={medication}";
            var document = await _context.OpenAsync(source);
            var elements = document.QuerySelectorAll("tr");

            var rxPrices = new List<RxPrice>();

            foreach (var e in elements)
            {
                int quantity;
                int.TryParse(e.QuerySelectorAll("span.table__price_bold")
                    .LastOrDefault()?.TextContent
                    .Replace(" Pills", ""), out quantity);

                int dose;
                int.TryParse(e.QuerySelectorAll("span.table__price_bold")
                    .FirstOrDefault()?.TextContent
                    .Replace("mg", ""), out dose);

                float price;
                float.TryParse(e.QuerySelector("div.table__price_row")?.TextContent
                    .Trim('$', ' ', '\n')
                    .Replace("USD", ""), out price);

                if (price > 0)
                {
                    rxPrices.Add(new RxPrice
                    {
                        Name = medication.ToLower(),
                        Quantity = quantity,
                        Dose = dose,
                        Price = price,
                        Location = "online",
                        VendorId = 2
                    });
                }
            }

            return rxPrices;
        }
    }
}
