using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Pricing;
using Core.Pricing.Abstractions;
using Core.Pricing.InMemoryProviders;
using Core.Services;
using Core.Strategies;
using FluentAssertions;
using Moq;

namespace Tests.ServicesTests;

public class RentalsServiceTests
{
    private readonly Mock<IRepository<RentalEntity, Guid>> _rentalRepoMock;
    private readonly Mock<IRepository<CarEntity, Guid>> _carRepoMock;
    private readonly RentalsService _sut;

    public RentalsServiceTests()
    {
        _rentalRepoMock = new Mock<IRepository<RentalEntity, Guid>>();
        _carRepoMock = new Mock<IRepository<CarEntity, Guid>>();

        var strategies = new IPriceStrategy[]
        {
        new SubcompactPriceStrategy(),
        new StationWagonPriceStrategy(),
        new TruckPriceStrategy()
        };

        var resolver = new PriceStrategyResolver(strategies);
        var calculator = new PriceCalculator(resolver);
        var priceProvider = new InMemoryPriceProvider();

        _sut = new RentalsService(
            _rentalRepoMock.Object,
            _carRepoMock.Object,
            priceProvider,
            calculator);
    }

    [Fact]
    public async Task Create_Should_Calculate_Total_Cost_And_Call_Repository()
    {
        var carId = Guid.NewGuid();
        var rental = new Rental
        {
            Id = Guid.NewGuid(),
            PersonalIdentification = "SomeRandomId",
            CarIdLicense = new () { Id = carId },
            RentalStart = DateTimeOffset.UtcNow.AddDays(-2),
            RentalFinish = DateTimeOffset.UtcNow,
            CarStartMilageInKm = 100
        };

        _carRepoMock.Setup(r => r.GetByIdAsync(carId))
            .ReturnsAsync(new CarEntity { Id = carId, CarType = CarTypeEnum.Subcompact });

        _rentalRepoMock.Setup(r => r.AddAsync(It.IsAny<RentalEntity>()))
            .ReturnsAsync((RentalEntity e) => e);

        var result = await _sut.Create(rental);

        result.Should().NotBeNull();
        result.TotalCost.Should().BePositive();

        _rentalRepoMock.Verify(r => r.AddAsync(It.IsAny<RentalEntity>()), Times.Once);
        _carRepoMock.Verify(r => r.GetByIdAsync(carId), Times.Once);
    }

    [Fact]
    public async Task Delete_Should_Call_Repository_And_Return_Rental()
    {
        var id = Guid.NewGuid();
        var entity = new RentalEntity { Id = id, PersonalIdentification = "SomeRandomId", CarIdLicense = new() { Id = Guid.NewGuid() } };

        _rentalRepoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(entity);

        var result = await _sut.Delete(id);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        _rentalRepoMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task Get_All_Active_Rentals_Should_Return_Active_Rentals()
    {
        var entities = new List<RentalEntity>
        {
            new RentalEntity { Id = Guid.NewGuid(), PersonalIdentification = "SomeRandomId1", CarIdLicense = new() { Id = Guid.NewGuid() } },
            new RentalEntity { Id = Guid.NewGuid(), PersonalIdentification = "SomeRandomId2", CarIdLicense = new() { Id = Guid.NewGuid() } }
        };

        _rentalRepoMock.Setup(r => r.ListAllAsync(It.IsAny<ISpecification<RentalEntity>>()))
            .ReturnsAsync(entities);

        var result = await _sut.GetAllActiveRentals();

        result.Should().HaveCount(2);
        _rentalRepoMock.Verify(r => r.ListAllAsync(It.IsAny<ISpecification<RentalEntity>>()), Times.Once);
    }

    [Fact]
    public async Task Get_Rental_Should_Return_Rental()
    {
        var id = Guid.NewGuid();
        var entity = new RentalEntity { Id = id, PersonalIdentification = "SomeRandomId", CarIdLicense = new() { Id = Guid.NewGuid() } };

        _rentalRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

        var result = await _sut.GetRental(id);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
    }

    [Fact]
    public async Task Start_Rental_Should_Set_Start_Date_When_Not_Started()
    {
        var id = Guid.NewGuid();
        var rentalEntity = new RentalEntity { Id = id, PersonalIdentification = "SomeRandomId", CarIdLicense = new() { Id = Guid.NewGuid() } };

        _rentalRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(rentalEntity);
        _rentalRepoMock.Setup(r => r.UpdateAsync(rentalEntity)).Returns(Task.CompletedTask);

        var result = await _sut.StartRental(id);

        result.RentalStart.Should().NotBe(DateTimeOffset.MinValue);
        _rentalRepoMock.Verify(r => r.UpdateAsync(rentalEntity), Times.Once);
    }

    [Fact]
    public async Task Finish_Rental_Should_Set_Finish_Date_And_Mileage_When_Not_Finished()
    {
        var id = Guid.NewGuid();
        var rentalEntity = new RentalEntity
        {
            Id = id,
            PersonalIdentification = "SomeRandomId",
            RentalStart = DateTimeOffset.UtcNow.AddDays(-1),
            CarIdLicense = new () { Id = Guid.NewGuid() }
        };

        _carRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new CarEntity { Id = rentalEntity.CarIdLicense.Id, CarType = CarTypeEnum.StationWagon });

        _rentalRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(rentalEntity);
        _rentalRepoMock.Setup(r => r.UpdateAsync(rentalEntity)).Returns(Task.CompletedTask);

        var milage = 500m;
        var result = await _sut.FinishRental(id, milage);

        result.RentalFinish.Should().NotBeNull();
        result.CarFinishMilageInKm.Should().Be(milage);
        _rentalRepoMock.Verify(r => r.UpdateAsync(rentalEntity), Times.Once);
    }

    [Fact]
    public async Task Update_Should_Modify_Entity_And_Calculate_Total_Cost()
    {
        var carId = Guid.NewGuid();
        var rentalEntity = new RentalEntity
        {
            Id = Guid.NewGuid(),
            PersonalIdentification = "SomeRandomId",
            RentalStart = DateTimeOffset.UtcNow.AddDays(-2),
            RentalFinish = DateTimeOffset.UtcNow,
            CarStartMilageInKm = 100,
            CarIdLicense = new IdValuePair<Guid>() { Id = carId }
        };

        _rentalRepoMock.Setup(r => r.GetByIdAsync(rentalEntity.Id))
            .ReturnsAsync(rentalEntity);

        _carRepoMock.Setup(r => r.GetByIdAsync(carId))
            .ReturnsAsync(new CarEntity { Id = carId, CarType = CarTypeEnum.Truck });

        _rentalRepoMock.Setup(r => r.UpdateAsync(rentalEntity)).Returns(Task.CompletedTask);

        var model = new Rental
        {
            Id = rentalEntity.Id,
            PersonalIdentification = "SomeRandomId",
            RentalStart = rentalEntity.RentalStart,
            RentalFinish = rentalEntity.RentalFinish,
            CarStartMilageInKm = 100,
            CarFinishMilageInKm = 200,
            CarIdLicense = new () { Id = carId }
        };

        var result = await _sut.Update(model);

        result.TotalCost.Should().BePositive();
        rentalEntity.CarFinishMilageInKm.Should().Be(200);
        _rentalRepoMock.Verify(r => r.UpdateAsync(rentalEntity), Times.Once);
    }

    [Fact]
    public async Task Create_Should_HandleNullModel()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Create(null!));
    }

    [Fact]
    public async Task Update_Should_Throw_WhenEntityNotFound()
    {
        var model = new Rental { Id = Guid.NewGuid(), PersonalIdentification = "SomeRandomId", CarIdLicense = new() { Id= Guid.NewGuid()}
        };
        _rentalRepoMock.Setup(r => r.GetByIdAsync(model.Id)).ThrowsAsync(new ArgumentNullException());

        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Update(model));
    }

    [Fact]
    public async Task StartRental_Should_NotChange_WhenAlreadyStarted()
    {
        var id = Guid.NewGuid();
        var rentalEntity = new RentalEntity { Id = id, RentalStart = DateTimeOffset.UtcNow.AddDays(-1), PersonalIdentification = "SomeRandomId", CarIdLicense = new() { Id = Guid.NewGuid()}
        };
        _rentalRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(rentalEntity);

        var result = await _sut.StartRental(id);

        result.RentalStart.Should().Be(rentalEntity.RentalStart);
        _rentalRepoMock.Verify(r => r.UpdateAsync(It.IsAny<RentalEntity>()), Times.Never);
    }

    [Fact]
    public async Task FinishRental_Should_NotChange_WhenAlreadyFinished()
    {
        var id = Guid.NewGuid();
        var rentalEntity = new RentalEntity
        {
            Id = id,
            CarIdLicense = new() { Id = Guid.NewGuid()},
            PersonalIdentification = "SomeRandomId",
            RentalStart = DateTimeOffset.UtcNow.AddDays(-2),
            RentalFinish = DateTimeOffset.UtcNow,
            CarFinishMilageInKm = 200
        };
        _rentalRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(rentalEntity);

        var result = await _sut.FinishRental(id, 500);

        result.RentalFinish.Should().Be(rentalEntity.RentalFinish);
        result.CarFinishMilageInKm.Should().Be(200);
        _rentalRepoMock.Verify(r => r.UpdateAsync(It.IsAny<RentalEntity>()), Times.Never);
    }

    [Fact]
    public async Task Start_Rental_Should_Throw_When_Rental_Not_Found()
    {
        var id = Guid.NewGuid();
        _rentalRepoMock.Setup(r => r.GetByIdAsync(id)).ThrowsAsync(new ArgumentNullException());

        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.StartRental(id));
    }

    [Fact]
    public async Task Finish_Rental_Should_Throw_When_Rental_Not_Found()
    {
        var id = Guid.NewGuid();
        _rentalRepoMock.Setup(r => r.GetByIdAsync(id)).ThrowsAsync(new ArgumentNullException());

        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.FinishRental(id, 100));
    }

    [Fact]
    public async Task Get_Rental_Should_Throw_When_Not_Found()
    {
        var id = Guid.NewGuid();
        _rentalRepoMock.Setup(r => r.GetByIdAsync(id)).ThrowsAsync(new ArgumentNullException());

        await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.GetRental(id));
    }
}