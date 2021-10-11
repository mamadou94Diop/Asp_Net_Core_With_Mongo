using System;
using System.Collections.Generic;
using DriveMeShop.Entity;
using MongoDB.Driver;

namespace DriveMeShop.Repository.implementation
{
    public class CarRepository: ICarRepository
    {
        private readonly IMongoCollection<Car> carCollection;

        public CarRepository(IMongoCollection<Car> mongoCollection)
        {
            carCollection = mongoCollection;
        }

        public async System.Threading.Tasks.Task<string> CreateAsync(Car car)
        {
            await carCollection.InsertOneAsync(car);

            return car.Id;
        }

        public Car GetCar(string id)
        {
            try
            {
                return carCollection.Find(car => car.Id == id).FirstOrDefault();
            } catch (FormatException exception)
            {
                throw new FormatException("The format of the id is not correct");
            }
        }

        public List<Car> GetCars(int? minimalReleasedYear, int? maximalReleasedYear)
        {
            return carCollection.Find(car => (minimalReleasedYear == null || car.ReleasedYear >= minimalReleasedYear) &&
                                             (maximalReleasedYear == null || car.ReleasedYear <= maximalReleasedYear)
                                ).ToList();
        }
    }
}
