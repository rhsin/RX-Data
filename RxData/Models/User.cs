using System.Collections.Generic;

namespace RxData.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<RxPriceUser> RxPriceUsers { get; set; }
    }
}
