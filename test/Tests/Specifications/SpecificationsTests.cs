using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Core.Specifications;
using FluentAssertions;
using Tests.Specifications.Models;

namespace Tests.Specifications
{
    public class SpecificationsTests
    {
        [Fact]
        public void Get_Query_Should_Apply_Criteria()
        {
            //Arrange
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var data = new List<CarEntity>
            {
                new() { Id = guid1, CurrentMilageInKm = 99 },
                new() { Id = guid2, CurrentMilageInKm = 299 }
            }.AsQueryable();
            
            var spec = new TestCarSpecification(200);
            //Act
            var result = EfSpecificationEvaluator<CarEntity>.GetQuery(data, spec);
            //Assert
            result.Should().HaveCount(1);
            result.ToList().First().Id.Should().Be(guid2);

        }
        
    }
}
