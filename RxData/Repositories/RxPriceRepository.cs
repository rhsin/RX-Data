using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RxData.Data;
using RxData.Models;
using RxData.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxData.Repositories
{
    public interface IRxPriceRepository
    {
        public Task<IEnumerable<RxPrice>> GetAll();
        public Task<IEnumerable<RxPrice>> GetMedication(string name);
        public Task<RxPriceDTO> FindBy(string name, string column, string value);
        public Task<RxPriceDTO> FindMedication(string name, string location, float price);
        public Task SeedRxPrices(string medication);
        public Task Create(RxPrice rxPrice);
        public Task Update(RxPrice rxPrice);
        public Task Delete(int id);
        public bool RxPricesSeeded(string medication);
    }

    public class RxPriceRepository : IRxPriceRepository
    {
        private readonly RxContext _context;
        private readonly IWebScraper _webScraper;
        private readonly IConfiguration _config;

        public RxPriceRepository(RxContext context, IWebScraper webScraper, IConfiguration config)
        {
            _context = context;
            _webScraper = webScraper;
            _config = config;
        }

        public async Task<IEnumerable<RxPrice>> GetAll()
        {
            return await _context.RxPrices.ToListAsync();
        }

        public async Task<IEnumerable<RxPrice>> GetMedication(string name)
        {
            return await _context.RxPrices
                .Where(rp => rp.Name == name)
                .ToListAsync();
        }

        public async Task<RxPriceDTO> FindBy(string name, string column, string value)
        {
            if (column.Length > 8)
            {
                throw new ArgumentException($"Invalid Column Argument: {column}!");
            }

            var parameters = new { Name = $"%{name}%", Value = value };

            var sql = $@"SELECT rp.Id, rp.Name, rp.Quantity, rp.Dose, rp.Price, 
                         rp.Location, v.Id AS VendorId, v.Name AS Vendor, v.Url
                         FROM RxPrices AS rp
                         INNER JOIN Vendors AS v
                         ON rp.VendorId = v.Id
                         WHERE LOWER(rp.Name) LIKE LOWER(@Name)
                         AND rp.{column} = @Value
                         ORDER BY rp.Price";

            using (var connection = new SqlConnection(_config.GetConnectionString("Default")))
            {
                var rxPrices = await connection.QueryAsync(sql, parameters);

                var rxPriceDTO = new RxPriceDTO
                {
                    Method = $"Find {name} By {column}: {value}",
                    Count = rxPrices.Count(),
                    RxPrices = rxPrices
                };

                return rxPriceDTO;
            }
        }

        public async Task<RxPriceDTO> FindMedication(string name, string location, float price)
        {
            var parameters = new { Name = $"%{name}%", Location = $"%{location}%", Price = price };

            var sql = @"SELECT rp.Id, rp.Name, rp.Quantity, rp.Dose, rp.Price, 
                        rp.Location, v.Id AS VendorId, v.Name AS Vendor, v.Url
                        FROM RxPrices AS rp
                        INNER JOIN Vendors AS v
                        ON rp.VendorId = v.Id
                        WHERE LOWER(rp.Name) LIKE LOWER(@Name)
                        AND LOWER(rp.Location) LIKE LOWER(@Location)
                        AND rp.Price <= @Price
                        ORDER BY rp.Price";

            using (var connection = new SqlConnection(_config.GetConnectionString("Default")))
            {
                var rxPrices = await connection.QueryAsync(sql, parameters);

                var rxPriceDTO = new RxPriceDTO
                {
                    Method = $"Get Medication: {name}",
                    Count = rxPrices.Count(),
                    RxPrices = rxPrices
                };

                return rxPriceDTO;
            }
        }

        public async Task SeedRxPrices(string medication)
        {
            var rxPrices = await _webScraper.GetRxPrices(medication);
            var rxPricesCanada = await _webScraper.GetRxPricesCanada(medication);
            var rxPricesCanadaAlt = await _webScraper.GetRxPricesCanadaAlt(medication);

            _context.RxPrices.AddRange(rxPrices);
            _context.RxPrices.AddRange(rxPricesCanada);
            _context.RxPrices.AddRange(rxPricesCanadaAlt);
            await _context.SaveChangesAsync();
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

        public bool RxPricesSeeded(string medication)
        {
            return _context.RxPrices.Any(rp => rp.Name == medication.ToLower());
        }
    }
}
