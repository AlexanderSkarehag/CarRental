using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;
using Infrastructure.Cosmos.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Cosmos.Contexts
{
    public class CarRentalContext : DbContext
    {
        public CarRentalContext(DbContextOptions<CarRentalContext> options) : base(options) { }

        DbSet<CarEntity> Car { get; set; }
        DbSet<RentalEntity> Rental { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildCars(modelBuilder);
            BuildRentals(modelBuilder);
        }

        private void BuildCars(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarEntity>().ToContainer(StartupExtension.DEFAULT_CAR_CONTAINER);
            modelBuilder.Entity<CarEntity>().HasKey(c => c.Id);
            modelBuilder.Entity<CarEntity>().HasPartitionKey(c => c.PartitionKey);
            modelBuilder.Entity<CarEntity>().UseETagConcurrency();
            modelBuilder.Entity<CarEntity>().HasDiscriminator(c => c.Discriminator);
#warning check if necessary.
            modelBuilder.Entity<CarEntity>().Property(c => c.Id).HasConversion(i => i.ToString(), i => new Guid(i));
        }

        private void BuildRentals(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RentalEntity>().ToContainer(StartupExtension.DEFAULT_RENTALS_CONTAINER).HasPartitionKey(r => r.PersonalIdentification);
            modelBuilder.Entity<RentalEntity>().HasKey(c => c.Id);
            modelBuilder.Entity<RentalEntity>().HasPartitionKey(c => c.PersonalIdentification);
#warning check if necessary.
            modelBuilder.Entity<RentalEntity>().Property(c => c.Id).HasConversion(i => i.ToString(), i => new Guid(i));
        }
    }
}
