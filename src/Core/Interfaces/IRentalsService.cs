using Core.Models;

namespace Core.Interfaces
{
    public interface IRentalsService
    {
        Task<Rental> Create(Rental model);
        Task<Rental> GetRental(Guid id);
        Task<IEnumerable<Rental>> GetAllActiveRentals();
        Task<Rental> Update(Rental model);
        Task<Rental> StartRental(Guid id);
        Task<Rental> FinishRental(Guid id, decimal milage);
        Task<Rental> Delete(Guid id);
    }
}
