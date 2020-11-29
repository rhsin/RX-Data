using AngleSharp;
using RxData.Models;
using System;
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
        public Task<IEnumerable<RxPrice>> GetRxPricesCanadaAlt(string medication);
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

            if (!elements.Any())
            {
                throw new ArgumentException($"Medication Not Found: {medication}!");
            }

            var textElements = document.QuerySelector("span.filter-amt--qty")?.TextContent.Split();

            var quantity = this.GetInteger(textElements.FirstOrDefault());
            var dose = this.GetInteger(textElements.LastOrDefault());

            var rxPrices = new List<RxPrice>();

            var vendor = new Vendor 
            {
                Name = "SingleCare",
                Url = "https://www.singlecare.com" 
            };

            foreach (var e in elements)
            {
                var price = this.GetFloat(e.QuerySelector("p.pharmacy-item__price")?.TextContent);
                var location = e.QuerySelector("img")?.GetAttribute("data-name");

                if (price > 0 && dose < 1000)
                {
                    rxPrices.Add(new RxPrice
                    {
                        Name = medication.ToLower(),
                        Quantity = quantity,
                        Dose = dose,
                        Price = price,
                        Location = location,
                        VendorId = 1,
                        Vendor = vendor
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

            if (!elements.Any())
            {
                throw new ArgumentException($"Medication Not Found: {medication}!");
            }

            var rxPrices = new List<RxPrice>();

            var vendor = new Vendor 
            {
                Name = "CanadaRx24h",
                Url = "https://canadarx24h.com"
            };

            foreach (var e in elements)
            {
                var quantity = this.GetInteger(e.QuerySelectorAll("span.table__price_bold")
                    .LastOrDefault()?.TextContent);

                var dose = this.GetInteger(e.QuerySelectorAll("span.table__price_bold")
                    .FirstOrDefault()?.TextContent);

                var price = this.GetFloat(e.QuerySelector("div.table__price_row")?.TextContent);

                if (price > 0 && dose < 1000)
                {
                    rxPrices.Add(new RxPrice
                    {
                        Name = medication.ToLower(),
                        Quantity = quantity,
                        Dose = dose,
                        Price = price,
                        Location = "online",
                        VendorId = 2,
                        Vendor = vendor
                    });
                }
            }

            return rxPrices;
        }

        public async Task<IEnumerable<RxPrice>> GetRxPricesCanadaAlt(string medication)
        {
            var source = $"https://www.onlinepharmaciescanada.com/pricedetail/{medication}";
            var document = await _context.OpenAsync(source);
            var elements = document.QuerySelectorAll("div#listproduct");

            if (!elements.Any())
            {
                throw new ArgumentException($"Medication Not Found: {medication}!");
            }

            var rxPrices = new List<RxPrice>();

            var vendor = new Vendor 
            { 
                Name = "OnlinePharmCanada",
                Url = "https://www.onlinepharmaciescanada.com" 
            };

            foreach (var e in elements)
            {
                var quantity = this.GetInteger(e.QuerySelector("div.productqty")?.TextContent);
                var dose = this.GetInteger(e.QuerySelector("div.productdose")?.TextContent);
                var price = this.GetFloat(e.QuerySelector("div.productprice")?.TextContent);

                if (price > 0 && dose < 1000)
                {
                    rxPrices.Add(new RxPrice
                    {
                        Name = medication.ToLower(),
                        Quantity = quantity,
                        Dose = dose,
                        Price = price,
                        Location = "online",
                        VendorId = 1002,
                        Vendor = vendor
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
