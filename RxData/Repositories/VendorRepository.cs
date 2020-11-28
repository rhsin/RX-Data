using Microsoft.EntityFrameworkCore;
using RxData.Data;
using RxData.DTO;
using RxData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxData.Repositories
{
    public interface IVendorRepository
    {
        public Task<VendorDTO> GetAll();
        public Task<VendorDTO> FindBy(string medication, string location);
        public Task SeedVendors();
        public Task Create(Vendor vendor);
        public Task Update(Vendor vendor);
        public Task Delete(int id);
        public bool VendorsSeeded();
    }

    public class VendorRepository : IVendorRepository
    {
        private readonly RxContext _context;

        public VendorRepository(RxContext context)
        {
            _context = context;
        }

        public async Task<VendorDTO> GetAll()
        {
            var vendors = await _context.Vendors
                .Include(v => v.RxPrices)
                .ToListAsync();

            var vendorDTO = new VendorDTO
            {
                Method = $"Get All Vendors",
                Count = vendors.Count(),
                Vendors = vendors
            };

            return vendorDTO;
        }

        public async Task<VendorDTO> FindBy(string medication, string location)
        {
            var vendors = await _context.Vendors
                .Include(v => v.RxPrices)
                .Select(v => new Vendor
                {
                    Id = v.Id,
                    Name = v.Name,
                    Url = v.Url,
                    RxPrices = v.RxPrices
                        .Where(rp => rp.Name.ToLower().Contains(medication.ToLower()))
                        .Where(rp => rp.Location.ToLower().Contains(location.ToLower()))
                        .OrderBy(rp => rp.Name)
                        .ToList()
                })
                .ToListAsync();

            var vendorDTO = new VendorDTO
            {
                Method = $"Find Vendors By: {medication}, {location}",
                Count = vendors.Count(),
                Vendors = vendors
            };

            return vendorDTO;
        }

        public async Task SeedVendors()
        {
            var vendors = new List<Vendor>
            {
                new Vendor { Name = "SingleCare", Url = "https://www.singlecare.com" },
                new Vendor { Name = "CanadaRx24h", Url = "https://canadarx24h.com" },
                new Vendor { Name = "OnlinePharmCanada", Url = "https://www.onlinepharmaciescanada.com" }
            };

            _context.Vendors.AddRange(vendors);
            await _context.SaveChangesAsync();
        }

        public async Task Create(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.Database.OpenConnectionAsync();

            try
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Vendors ON");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Vendors OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        public async Task Update(Vendor vendor)
        {
            _context.Entry(vendor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
        }

        public bool VendorsSeeded()
        {
            return _context.Vendors.Any();
        }
    }
}
