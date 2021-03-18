using System.Threading.Tasks;
using DriveMeShop.Entity;

namespace DriveMeShop.Repository
{
    public interface ICarRepository
    {
        public Task<string> CreateAsync(Car car);
    }
}
