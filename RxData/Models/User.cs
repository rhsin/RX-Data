using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RxData.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }

        public ICollection<RxPriceUser> RxPriceUsers { get; set; }
    }
}
