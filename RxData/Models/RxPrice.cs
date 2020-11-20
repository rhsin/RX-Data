
namespace RxData.Models
{
    public class RxPrice
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int Dose { get; set; }
        public float Price { get; set; }
        public string Location { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}
