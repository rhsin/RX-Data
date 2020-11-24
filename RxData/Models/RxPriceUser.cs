
namespace RxData.Models
{
    public class RxPriceUser
    {
        public int RxPriceId { get; set; }
        public RxPrice RxPrice { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
