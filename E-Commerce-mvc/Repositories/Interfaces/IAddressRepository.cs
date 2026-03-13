using E_Commerce_mvc.Models.Entities;

namespace E_Commerce_mvc.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<IEnumerable<Address>> GetByUserIdAsync(string userId);
        Task<Address?> GetByIdAsync(int id);
        Task AddAsync(Address address);
        void Update(Address address);
        void Delete(Address address);
        Task SaveChangesAsync();
    }
}
