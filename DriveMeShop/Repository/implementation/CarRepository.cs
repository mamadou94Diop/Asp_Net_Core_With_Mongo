using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriveMeShop.Entity;
using MongoDB.Driver;
using static MongoDB.Driver.ReplaceOneResult;

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

        public async Task<string> UpdateCarAsync(Car car)
        {
            try
            {
                var replaceResult = await carCollection.ReplaceOneAsync((_car => car.Id == _car.Id), car);
                if (replaceResult is Acknowledged)
                {
                    return car.Id;
                }
                else
                {
                    return null;
                }
            }
            catch (FormatException exception)
            {
                throw new FormatException("The format of the id is not correct");

            }
        }

        public async Task<string> UpdateCarLastRevisionYearAsync(string id, int? lastRevisionYear)
        {
            var filter = Builders<Car>.Filter.Eq(car => car.Id, id);
            var update = Builders<Car>.Update.Set(car => car.LastRevisionYear, lastRevisionYear);

            var updateResult = await carCollection.UpdateOneAsync(filter, update);

            return updateResult.IsAcknowledged ? id : null;
        }

        public async Task<bool> DeleteCarAsync(string id)
        {
            DeleteResult deleteResult = await carCollection.DeleteOneAsync(car => car.Id == id);

            if (deleteResult.IsAcknowledged && deleteResult.DeletedCount == 1)
                return true;


            return false;

        }
    }
}
