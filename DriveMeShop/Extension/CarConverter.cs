using DriveMeShop.Entity;
using DriveMeShop.Model;

namespace DriveMeShop.Extension
{
    public static class CarConverter
    {
       public static Car ToCar(this CarModel carModel)
        {
            return new Car {
                Id = carModel.Id,
                Make = carModel.Make,
                Model = carModel.Model,
                Mileage = carModel.Mileage,
                ReleasedYear = carModel.ReleasedYear,
                LastRevisionYear = carModel.LastRevisionYear,
                TransmissionMode = carModel.IsTransmissionAutomatic ? "AUTOMATIC" : "MANUAL"

            };

        }

        public static CarModel ToCarModel(this Car car)
        {
            return new CarModel
            {
                Id = car.Id,
                Make = car.Make,
                Model = car.Model,
                Mileage = car.Mileage,
                ReleasedYear = car.ReleasedYear,
                LastRevisionYear = car.LastRevisionYear,
                IsTransmissionAutomatic = car.TransmissionMode == "AUTOMATIC"

            };
        }
    }
}
