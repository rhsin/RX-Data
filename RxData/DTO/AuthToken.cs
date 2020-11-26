using RxData.Models;

namespace RxData.DTO
{
    public class AuthToken
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
