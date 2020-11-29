using Microsoft.EntityFrameworkCore;
using RxData.Data;
using RxData.DTO;
using RxData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxData.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserDTO>> GetAll();
        public Task AddRxPrice(int rxPriceId, int userId);
        public Task RemoveRxPrice(int rxPriceId, int userId);
        public Task Create(UserDTO userDTO);
        public Task Update(UserDTO userDTO);
        public Task Delete(int id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly RxContext _context;

        public UserRepository(RxContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            return await _context.Users
                .Include(u => u.RxPriceUsers)
                .ThenInclude(ru => ru.RxPrice)
                .ThenInclude(rp => rp.Vendor)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    RxPrices = u.RxPriceUsers
                })
                .ToListAsync();
        }

        public async Task AddRxPrice(int rxPriceId, int userId)
        {
            _context.RxPriceUsers.Add(new RxPriceUser 
            {
                RxPriceId = rxPriceId,
                UserId = userId 
            });

            await _context.SaveChangesAsync();
        }

        public async Task RemoveRxPrice(int rxPriceId, int userId)
        {
            _context.RxPriceUsers.Remove(new RxPriceUser
            {
                RxPriceId = rxPriceId,
                UserId = userId
            });

            await _context.SaveChangesAsync();
        }

        public async Task Create(UserDTO userDTO)
        {
            var user = new User
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                Email = userDTO.Email,
                Role = "User"
            };

            _context.Users.Add(user);
            await _context.Database.OpenConnectionAsync();

            try
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Users ON");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Users OFF");
            }
            finally
            {
                await _context.Database.CloseConnectionAsync();
            }
        }

        public async Task Update(UserDTO userDTO)
        {
            var user = new User
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                Email = userDTO.Email,
                Role = "User"
            };

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
