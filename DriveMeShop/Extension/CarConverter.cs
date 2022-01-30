using DriveMeShop.Entity;
using DriveMeShop.Model;

namespace DriveMeShop.Extension
{
    public static class CarConverter
    {
       public static Car ToCar(this IdentifiedCarModel carModel)
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

        public static Car ToCar(this UnidentifiedCarModel carModel)
        {
            return new Car
            {
                Make = carModel.Make,
                Model = carModel.Model,
                Mileage = carModel.Mileage,
                ReleasedYear = carModel.ReleasedYear,
                LastRevisionYear = carModel.LastRevisionYear,
                TransmissionMode = carModel.IsTransmissionAutomatic ? "AUTOMATIC" : "MANUAL"

            };

        }

        public static IdentifiedCarModel ToIdentifiedCarModel(this Car car)
        {
            return new IdentifiedCarModel
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
