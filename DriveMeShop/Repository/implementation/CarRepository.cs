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
    }
}
