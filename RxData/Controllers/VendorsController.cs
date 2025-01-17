﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RxData.DTO;
using RxData.Models;
using RxData.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace RxData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorsController(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }

        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<VendorDTO>> GetVendors()
        {
            return Ok(await _vendorRepository.GetAll());
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            var vendors = await _vendorRepository.GetAll();
            var vendor = vendors.Vendors.FirstOrDefault(vendor => vendor.Id == id);

            if (vendor == null)
            {
                return NotFound();
            }

            return vendor;
        }

        // GET: api/Vendors/Find
        [HttpGet("Find")]
        public async Task<ActionResult<VendorDTO>> FindVendors(string medication,
            string location)
        {
            return Ok(await _vendorRepository.FindBy(medication, location));
        }

        // POST: api/Vendors/Seeder
        [Authorize(Roles = "Admin")]
        [HttpPost("Seeder")]
        public async Task<ActionResult<string>> SeedVendors()
        {
            if (_vendorRepository.VendorsSeeded())
            {
                return BadRequest("Vendors Already Seeded!");
            }

            await _vendorRepository.SeedVendors();

            return Ok("Vendors Seeded Successfully!");
        }

        // PUT: api/Vendors/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest();
            }

            await _vendorRepository.Update(vendor);

            return NoContent();
        }

        // POST: api/Vendors
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(Vendor vendor)
        {
            await _vendorRepository.Create(vendor);

            return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
        }

        // DELETE: api/Vendors/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vendor>> DeleteVendor(int id)
        {
            await _vendorRepository.Delete(id);

            return NoContent();
        }
    }
}
