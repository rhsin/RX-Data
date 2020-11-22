using AngleSharp;
using RxData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RxData.Services
{
    public interface IWebScraper
    {
        public Task<IEnumerable<RxPrice>> GetRxPrices(string medication);
        public Task<IEnumerable<RxPrice>> GetRxPricesCanada(string medication);
        public int GetInteger(string input);
        public float GetFloat(string input);
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
            var text = document.QuerySelector("span.filter-amt--qty")?.TextContent.Split();

            int quantity = this.GetInteger(text.FirstOrDefault());

            int dose = this.GetInteger(text.LastOrDefault());

            var rxPrices = new List<RxPrice>();

            foreach (var e in elements)
            {
                float price = this.GetFloat(e.QuerySelector("p.pharmacy-item__price")?.TextContent);

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
                int quantity = this.GetInteger(e.QuerySelectorAll("span.table__price_bold")
                    .LastOrDefault()?.TextContent);

                int dose = this.GetInteger(e.QuerySelectorAll("span.table__price_bold")
                    .FirstOrDefault()?.TextContent);

                float price = this.GetFloat(e.QuerySelector("div.table__price_row")?.TextContent);

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

        public int GetInteger(string input)
        {
            if (input == null)
            {
                return 0;
            }

            var regex = new Regex(@"[^\d.]");

            int result;
            int.TryParse(regex.Replace(input, ""), out result);

            return result;
        }

        public float GetFloat(string input)
        {
            if (input == null)
            {
                return 0;
            }

            var regex = new Regex(@"[^\d.]");

            float result;
            float.TryParse(regex.Replace(input, ""), out result);

            return result;
        }
    }
}
