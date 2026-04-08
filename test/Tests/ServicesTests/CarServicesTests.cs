using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Services;
using FluentAssertions;
using Moq;

namespace Tests.ServicesTests;

public class CarsServiceTests
{
    private readonly Mock<IRepository<CarEntity, Guid>> _carRepoMock;
    private readonly Mock<IRentalsService> _rentalsServiceMock;
    private readonly CarsService _carsService;

    public CarsServiceTests()
    {
        _carRepoMock = new Mock<IRepository<CarEntity, Guid>>();
        _rentalsServiceMock = new Mock<IRentalsService>();
        _carsService = new CarsService(_carRepoMock.Object, _rentalsServiceMock.Object);
    }

    [Fact]
    public async Task Create_Should_Throw_When_Car_Type_Is_Unknown()
    {
        //Arrange
        var model = new Car { CarType = CarTypeEnum.Unknown, LicensePlate = "Test123" };
        //Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _carsService.Create(model));
    }

    [Fact]
    public async Task Create_Should_Call_Repository_And_Return_Car()
    {
        //Arrange
        var model = new Car { CarType = CarTypeEnum.Subcompact, LicensePlate = "Test123" };
        _carRepoMock
            .Setup(r => r.AddAsync(It.IsAny<CarEntity>()))
            .ReturnsAsync((CarEntity e) => e);

        //Act
        var result = await _carsService.Create(model);
        //Assert
        result.Should().NotBeNull();
        result.CarType.Should().Be(model.CarType);
        _carRepoMock.Verify(r => r.AddAsync(It.IsAny<CarEntity>()), Times.Once);
    }

    [Fact]
    public async Task Update_Should_Modify_Milage_And_Call_Repository()
    {
        //Arrange
        var id = Guid.NewGuid();
        var license = "Test123";
        var entity = new CarEntity { Id = id, CurrentMilageInKm = 100, LicensePlate = license };
        _carRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);
        _carRepoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);

        var model = new Car { Id = id, CurrentMilageInKm = 200, LicensePlate = license };
        //Act
        await _carsService.Update(model);
        //Assert
        entity.CurrentMilageInKm.Should().Be(200);
        _carRepoMock.Verify(r => r.UpdateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task GetAllCars_ShouldReturnAllCars()
    {
        var entities = new List<CarEntity>
        {
            new CarEntity { Id = Guid.NewGuid() },
            new CarEntity { Id = Guid.NewGuid() }
        };

        _carRepoMock.Setup(r => r.ListAll()).ReturnsAsync(entities);

        var result = await _carsService.GetAllCars();

        result.Should().HaveCount(2);
        result.Select(c => c.Id).Should().BeEquivalentTo(entities.Select(e => e.Id));
    }

    [Fact]
    public async Task Get_Available_Cars_Should_Call_Rentals_Service_And_Repository()
    {
        var unavailable = new List<Rental>
        {
            new Rental()
            {
                PersonalIdentification = "SomePredefinedId",
                CarIdLicense = new() { Id = Guid.NewGuid() },
                RentalStart = DateTimeOffset.MinValue
            }
        };

        _rentalsServiceMock
            .Setup(r => r.GetAllActiveRentals())
            .ReturnsAsync(unavailable);

        _carRepoMock
            .Setup(r => r.ListAllAsync(It.IsAny<ISpecification<CarEntity>>()))
            .ReturnsAsync([]);

        var result = await _carsService.GetAvailableCars();

        result.Should().BeEmpty();
        _rentalsServiceMock.Verify(r => r.GetAllActiveRentals(), Times.Once);
        _carRepoMock.Verify(r => r.ListAllAsync(It.IsAny<ISpecification<CarEntity>>()), Times.Once);
    }

    [Fact]
    public async Task Get_Car_Should_Return_Car()
    {
        var id = Guid.NewGuid();
        var entity = new CarEntity { Id = id };

        _carRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(entity);

        var result = await _carsService.GetCar(id);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
    }

    [Fact]
    public async Task Delete_Should_Call_Repository_And_Return_Car()
    {
        var id = Guid.NewGuid();
        var entity = new CarEntity { Id = id };

        _carRepoMock.Setup(r => r.DeleteAsync(id)).ReturnsAsync(entity);

        var result = await _carsService.Delete(id);

        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        _carRepoMock.Verify(r => r.DeleteAsync(id), Times.Once);
    }

    [Fact]
    public async Task Create_Should_Throw_When_Car_Type_Unknown()
    {
        var car = new Car { CarType = CarTypeEnum.Unknown };
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _carsService.Create(car));
    }

    [Fact]
    public async Task Get_Car_Should_Throw_When_Car_Not_Found()
    {
        var carId = Guid.NewGuid();
        _carRepoMock.Setup(r => r.GetByIdAsync(carId)).ThrowsAsync(new ArgumentNullException());

        await Assert.ThrowsAsync<ArgumentNullException>(() => _carsService.GetCar(carId));
    }
}