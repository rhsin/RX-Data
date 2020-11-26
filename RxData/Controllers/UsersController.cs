using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RxData.Models;
using RxData.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxData.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _userRepository.GetAll());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var users = await _userRepository.GetAll();
            var user = users.FirstOrDefault(user => user.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users/RxPrices/2/1
        [HttpPost("RxPrices/{rxPriceId}/{userId}")]
        public async Task<ActionResult<string>> AddRxPrice(int rxPriceId, int userId)
        {
            await _userRepository.AddRxPrice(rxPriceId, userId);

            return Ok($"RxPrice {rxPriceId} Added To User {userId}!");
        }

        // DELETE: api/Users/RxPrices/2/1
        [HttpDelete("RxPrices/{rxPriceId}/{userId}")]
        public async Task<ActionResult<string>> RemoveRxPrice(int rxPriceId, int userId)
        {
            await _userRepository.RemoveRxPrice(rxPriceId, userId);

            return Ok($"RxPrice {rxPriceId} Removed From User {userId}!");
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            await _userRepository.Update(user);

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            await _userRepository.Create(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            await _userRepository.Delete(id);

            return NoContent();
        }
    }
}
