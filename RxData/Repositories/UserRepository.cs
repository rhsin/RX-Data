using Microsoft.EntityFrameworkCore;
using RxData.Data;
using RxData.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RxData.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAll();
        public Task AddRxPrice(int rxPriceId, int userId);
        public Task RemoveRxPrice(int rxPriceId, int userId);
        public Task Create(User user);
        public Task Update(User user);
        public Task Delete(int id);
    }

    public class UserRepository : IUserRepository
    {
        private readonly RxContext _context;

        public UserRepository(RxContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .Include(u => u.RxPriceUsers)
                .ThenInclude(rp => rp.RxPrice)
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

        public async Task Create(User user)
        {
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

        public async Task Update(User user)
        {
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
