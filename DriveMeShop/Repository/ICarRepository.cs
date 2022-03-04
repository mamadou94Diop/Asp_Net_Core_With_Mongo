using System.Collections.Generic;
using System.Threading.Tasks;
using DriveMeShop.Entity;

namespace DriveMeShop.Repository
{
    public interface ICarRepository
    {
        public Task<string> CreateAsync(Car car);
        public List<Car> GetCars(int? minimalReleasedYear, int? maximalReleasedYear);
        public Car GetCar(string id);
        public Task<string> UpdateCarAsync(Car car);
        public Task<string> UpdateCarLastRevisionYearAsync(string id, int? lastRevisionYear);
    }
}
