using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Core.Specifications.Cars;

namespace Core.Services
{
    public class CarsService : ICarsService
    {
        private readonly IRepository<CarEntity, Guid> _carRepository;
        private readonly IRentalsService _rentalsService;

        public CarsService(IRepository<CarEntity, Guid> carRepository, IRentalsService rentalsService)
        {
            _carRepository = carRepository;
            _rentalsService = rentalsService;
        }
        public async Task<Car> Create(Car model)
        {
            if (model.CarType == Enums.CarTypeEnum.Unknown)
                throw new ArgumentOutOfRangeException();

            var entity = await _carRepository.AddAsync(new CarEntity(model) { Id = Guid.NewGuid() });
            return new Car(entity);
        }

        public async Task<Car> Delete(Guid id)
        {
            var entity = await _carRepository.DeleteAsync(id);
            return new Car(entity);
        }
        public async Task<IEnumerable<Car>> GetAllCars()
        {
            var carEntities = await _carRepository.ListAll();
            IEnumerable<Car> cars = carEntities.Select(e => new Car(e));
            return cars;
        }
        public async Task<IEnumerable<Car>> GetAvailableCars()
        {
            var unavailable = await _rentalsService.GetAllActiveRentals();
            var spec = new CarsByIdsSpecification([.. unavailable.Select(r => r.CarIdLicense.Id)]);
            var carEntities = await _carRepository.ListAllAsync(spec);
            IEnumerable<Car> cars = carEntities.Select(e => new Car(e));
            return cars;
        }

        public async Task<Car> GetCar(Guid id)
        {
            var entity = await _carRepository.GetByIdAsync(id);
            return new Car(entity);
        }

        public async Task<Car> Update(Car model)
        {
            var entity = await _carRepository.GetByIdAsync(model.Id);

            entity.CurrentMilageInKm = model.CurrentMilageInKm;

            await _carRepository.UpdateAsync(entity);

            return new Car(entity);
        }
    }
}
