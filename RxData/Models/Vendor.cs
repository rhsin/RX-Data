using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RxData.Models
{
    public class Vendor
    {
        public int Id { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        [Required, Url]
        public string Url { get; set; }

        public ICollection<RxPrice> RxPrices { get; set; }
    }
}
