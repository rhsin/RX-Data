using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RxData.Models
{
    public class RxPrice
    {
        public int Id { get; set; }

        [Required, StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }

        [Range(0, 500)]
        public int Quantity { get; set; }

        [Range(0, 1000)]
        public int Dose { get; set; }

        [Range(0, 1000)]
        public float Price { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string Location { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public ICollection<RxPriceUser> RxPriceUsers { get; set; }
    }
}
