using System.Collections.Generic;

namespace RxData.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public ICollection<RxPrice> RxPrices { get; set; }
    }
}
