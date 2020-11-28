using RxData.Models;
using System.Collections.Generic;

namespace RxData.DTO
{
    public class VendorDTO
    {
        public string Method { get; set; }
        public int Count { get; set; }

        public IEnumerable<Vendor> Vendors { get; set; }
    }
}
