using E_Commerce_mvc.Data;
using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_mvc.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetByUserIdAsync(string userId)
            => await _context.Addresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.IsDefault)
                .ToListAsync();

        public async Task<Address?> GetByIdAsync(int id)
            => await _context.Addresses.FindAsync(id);

        public async Task AddAsync(Address address)
            => await _context.Addresses.AddAsync(address);

        public void Update(Address address)
            => _context.Addresses.Update(address);

        public void Delete(Address address)
            => _context.Addresses.Remove(address);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
