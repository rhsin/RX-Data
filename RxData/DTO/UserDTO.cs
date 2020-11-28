using RxData.Models;
using System.Collections.Generic;

namespace RxData.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<RxPriceUser> RxPrices { get; set; }
    }
}
