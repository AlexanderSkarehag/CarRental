using Core.Models;

namespace Core.Interfaces
{
    public interface ICarsService
    {
        Task<Car> Create(Car model);
        Task<Car> GetCar(Guid id);
        Task<IEnumerable<Car>> GetAvailableCars();
        Task<Car> Update(Car model);
        Task<Car> Delete(Guid id);
    }
}
