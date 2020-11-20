
namespace RxData.Models
{
    public class RxPrice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}
