using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Core.Repositories;
using FluentAssertions;
using Infrastructure.Cosmos.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class EFRepositoryTests
    {

        [Fact]
        public async Task Add_Async_Should_Add_Entity()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<CarRentalContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var context = new CarRentalContext(options);
            var repo = new EfRepository<CarEntity, Guid, CarRentalContext>(context);

            var entity = new CarEntity { Id = Guid.NewGuid(), LicensePlate = "ABC123" };
            //Act
            var result = await repo.AddAsync(entity);

            //Assert
            context.Set<CarEntity>().Should().ContainSingle();

        }
    }
}
