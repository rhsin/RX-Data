﻿using Microsoft.AspNetCore.Mvc;
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
    public class VendorsController : ControllerBase
    {
        private readonly RxContext _context;

        public VendorsController(RxContext context)
        {
            _context = context;
        }

        // POST: api/Vendors/Seeder
        [HttpPost("Seeder")]
        public async Task<ActionResult<string>> SeedVendors()
        {
            if (_context.Vendors.Any())
            {
                return BadRequest("Vendors Already Seeded!");
            }

            var vendors = new List<Vendor>
            {
                new Vendor { Name = "SingleCare", Url = "https://www.singlecare.com" },
                new Vendor { Name = "CanadaRx24h", Url = "https://canadarx24h.com/" }
            };

            _context.Vendors.AddRange(vendors);
            await _context.SaveChangesAsync();

            return Ok("Vendors Seeded Successfully!");
        }

        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vendor>>> GetVendors()
        {
            return await _context.Vendors.ToListAsync();
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);

            if (vendor == null)
            {
                return NotFound();
            }

            return vendor;
        }

        // PUT: api/Vendors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        // POST: api/Vendors
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vendor>> DeleteVendor(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }

            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();

            return vendor;
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.Id == id);
        }
    }
}