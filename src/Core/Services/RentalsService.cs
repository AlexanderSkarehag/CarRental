using Core.Consts;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Pricing;
using Core.Pricing.Abstractions;
using Core.Specifications.Rentals;

namespace Core.Services
{
    public class RentalsService : IRentalsService
    {
        private readonly IRepository<RentalEntity, Guid> _rentalRepository;
        private readonly IRepository<CarEntity, Guid> _carRepository;
        private readonly IPriceProvider _priceProvider;
        private readonly PriceCalculator _priceCalculator;

        public RentalsService(
            IRepository<RentalEntity, Guid> rentalRepository, 
            IRepository<CarEntity, Guid> carRepository, 
            IPriceProvider priceProvider, 
            PriceCalculator priceCalculator)
        {
            _rentalRepository = rentalRepository;
            _carRepository = carRepository;
            _priceProvider = priceProvider;
            _priceCalculator = priceCalculator;
        }
        public async Task<Rental> Create(Rental model)
        {
            ArgumentNullException.ThrowIfNull(model);

            var activeRentals = await _rentalRepository.ListAllAsync(new GetActiveRentalByIdSpecification(model.CarIdLicense.Id));
            if (activeRentals.Count > 0)
                throw new CarIsAlreadyRentedException();

            model.TotalCost = await CalculateTotalCostAsync(model.RentalStart, model.RentalFinish, model.CarIdLicense.Id, (model.CarFinishMilageInKm ?? model.CarStartMilageInKm - model.CarStartMilageInKm));
            var entity = await _rentalRepository.AddAsync(MapModelToEntity(model));
            return MapEntityToModel(entity);
        }

        public async Task<Rental> Delete(Guid id)
        {
            var entity = await _rentalRepository.DeleteAsync(id);
            return MapEntityToModel(entity);
        }

        public async Task<IEnumerable<Rental>> GetAllActiveRentals()
        {
            var spec = new ActiveRentalsSpecification();
            var RentalEntities = await _rentalRepository.ListAllAsync(spec);
            IEnumerable<Rental> rentals = RentalEntities.Select(MapEntityToModel);
            return rentals;
        }

        public async Task<Rental> GetRental(Guid id)
        {
            var entity = await _rentalRepository.GetByIdAsync(id);
            return MapEntityToModel(entity);
        }

        public async Task<Rental> StartRental(Guid id)
        {
            var rental = await GetRental(id);

            if (rental.RentalStart != DateTimeOffset.MinValue)
                return rental;

            rental.RentalStart = DateTimeOffset.UtcNow;
            return await Update(rental);
        }
        public async Task<Rental> FinishRental(Guid id, decimal mileage)
        {
            var rental = await GetRental(id);

            if (rental.RentalFinish != null && rental.RentalFinish != DateTimeOffset.MinValue)
                return rental;

            rental.RentalFinish = DateTimeOffset.UtcNow;
            rental.CarFinishMilageInKm = mileage;
            await BumpMileage(mileage, rental);
            return await Update(rental);
        }

        private async Task BumpMileage(decimal mileage, Rental rental)
        {
            var car = await _carRepository.GetByIdAsync(rental.CarIdLicense.Id);
            car.CurrentMilageInKm = mileage;
            await _carRepository.UpdateAsync(car);
        }

        public async Task<Rental> Update(Rental model)
        {
            var entity = await _rentalRepository.GetByIdAsync(model.Id);
            entity = await MapModelToEntity(model, entity);

            await _rentalRepository.UpdateAsync(entity);

            return MapEntityToModel(entity);
        }

        private async Task<decimal?> CalculateTotalCostAsync(DateTimeOffset start, DateTimeOffset? finish, Guid carId, decimal mileage)
        {
            //No start/end == no total
            if (start == DateTimeOffset.MinValue ||
                finish == null ||
                finish == DateTimeOffset.MinValue
               )
            {
                return null;
            }
            var car = await _carRepository.GetByIdAsync(carId);
            var totalDays = (decimal)(finish - start)!.Value.TotalDays;
            var request = new PriceCalculationRequest
            {
                Type = car.CarType,
                BaseDayPrice = _priceProvider.GetBaseDayPrice(car.CarType),
                BaseKmPrice = _priceProvider.GetBaseKmPrice(car.CarType),
                TotalDays = totalDays,
                TotalKm = mileage
            };
            return _priceCalculator.CalculatePrice(request);
        }
        private static Rental MapEntityToModel(RentalEntity entity)
        {
            return new Rental(entity) { CarIdLicense = entity.CarIdLicense, PersonalIdentification = entity.PersonalIdentification, TotalCost = entity.TotalCost };
        }
        private static RentalEntity MapModelToEntity(Rental model)
        {
            return new RentalEntity(model) { Id = Guid.NewGuid(), CarIdLicense = model.CarIdLicense, PersonalIdentification = model.PersonalIdentification };
        }
        private async Task<RentalEntity> MapModelToEntity(Rental model, RentalEntity entity)
        {
            entity.PersonalIdentification = model.PersonalIdentification;
            entity.RentalStart = model.RentalStart;
            entity.RentalFinish = model.RentalFinish;
            entity.CarFinishMilageInKm = model.CarFinishMilageInKm;
            entity.CarStartMilageInKm = model.CarStartMilageInKm;
            entity.CarIdLicense = model.CarIdLicense; //might switch car before heading out?
            entity.TotalCost = await CalculateTotalCostAsync(entity.RentalStart, entity.RentalFinish, entity.CarIdLicense.Id, (entity.CarFinishMilageInKm ?? entity.CarStartMilageInKm - entity.CarStartMilageInKm));

            return entity;
        }
    }
}
