using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DriveMeShop.Controllers;
using DriveMeShop.Entity;
using DriveMeShop.Model;
using DriveMeShop.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    public partial class CarsControllerTest
    {
      
        [Test]
        public async Task given_a_new_car_when_insert_succeeded_then_return_201_statusAsync()
        {
            //Arrange

            var carModel = new UnidentifiedCarModel
            {
                Make = "BMW",
                Model = "X6",
                ReleasedYear = 2020,
                IsTransmissionAutomatic = true,
                Mileage = 5000
            };

            var fakeCarId = Task.FromResult<String>("fhfhflhflhflhf");

            var carRepository = new Mock<ICarRepository>();

            carRepository.Setup (repository => repository.CreateAsync(It.IsAny<Car>()))
                         .Returns(fakeCarId);


            //Act

            var controller = new CarsController(carRepository.Object);

            var result = await controller.PostAsync(carModel);


            //Assert
            Assert.IsInstanceOf<CreatedResult>(result);
        }

        [Test]
        public async Task given_a_new_car_when_insert_throws_exception_then_return_500_status()
        {

            //Arrange
            var carModel = new UnidentifiedCarModel
            {
                Make = "BMW",
                Model = "X6",
                ReleasedYear = 2020,
                IsTransmissionAutomatic = true,
                Mileage = 5000
            };

            var fakeException = new Exception("Fatal error");

            var carRepository = new Mock<ICarRepository>();

            carRepository.Setup(repository => repository.CreateAsync(It.IsAny<Car>()))
                         .Throws(fakeException);


            //Act

            var controller = new CarsController(carRepository.Object);

            var result = await controller.PostAsync(carModel);

            //Assert
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void given_a_catalog_when_fetching_cars_is_successful_then_return_200_status()
        {
            //Arrange

            var mockCars = new List<Car>();
            mockCars.Add(new Car {
                Id= "112TVTVTVT",
                Make = "BMW",
                Model = "X6",
                ReleasedYear = 2020,
                TransmissionMode = "MANUAL",
                Mileage = 5000
            });

            var carRepository = new Mock<ICarRepository>();
            carRepository.Setup(repository => repository.GetCars(It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(mockCars);

            //Act

            var controller = new CarsController(carRepository.Object);
            var result = controller.Get(null, null);

            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void given_an_id_when_fetching_car_with_that_id_is_successful_then_return_200_status()
        {
            //Arrange
            var mockCar = new Car
            {
                Id = "112TVTVTVT",
                Make = "BMW",
                Model = "X6",
                ReleasedYear = 2020,
                TransmissionMode = "MANUAL",
                Mileage = 5000
            };

            var carRepository = new Mock<ICarRepository>();
            carRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Returns(mockCar);

            //Act
            var controller = new CarsController(carRepository.Object);
            var result = controller.Get("112TVTVTVT");

            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void given_an_id_when_fetching_car_with_that_id_returns_null_then_return_404_status()
        {
            //Arrange
            Car mockCar = null;

            var carRepository = new Mock<ICarRepository>();
            carRepository.Setup(repository => repository.GetCar(It.IsAny<string>() ) )
                .Returns(mockCar);

            //Act
            var controller = new CarsController(carRepository.Object);
            var result = controller.Get("112TVTVTVT");

            //Assert
            Assert.AreEqual(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public void given_an_invalid_id_when_trying_to_fetch_car_with_that_id_then_return_400_status()
        {
            //Arrange
            var exception = new FormatException("format exception");
            var carRepository = new Mock<ICarRepository>();
            carRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Throws(exception);


            //Act
            var controller = new CarsController(carRepository.Object);
            var result = controller.Get("1452555ADDDKD");


            //Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest,((ObjectResult)result).StatusCode);
        }
        
        public async Task given_a_car_without_id_when_updating_is_successful_then_return_201_status()
        {
            //Arrange
            var carmodel = new IdentifiedCarModel
            {
                Make = "Nissan",
                Model = "Qasqhai",
                Mileage = 135000,
                ReleasedYear = 2018,
                IsTransmissionAutomatic = true,
                LastRevisionYear = 2020
            };
            var mockCarId = "12273Jdnkdnkdn";

            var carRepository = new Mock<ICarRepository>();
            carRepository.Setup(repository => repository.CreateAsync(It.IsAny<Car>()))
                .Returns(Task.FromResult<string>(mockCarId));

            //Act
            var controller = new CarsController(carRepository.Object);
            var result = await controller.PutAsync(carmodel);

            //Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)result).StatusCode);

        }

        [Test]
        public async Task given_a_car_with_an_id_when_update_throws_format_exception_then_return_400_statusAsync()
        {
            //Arrange
            var carmodel = new IdentifiedCarModel
            {
                Id = "tfuuvjv133",
                Make = "Nissan",
                Model = "Qasqhai",
                Mileage = 135000,
                ReleasedYear = 2018,
                IsTransmissionAutomatic = true,
                LastRevisionYear = 2020
            };
            var existingCar = new Car
            {
                Id = "tfuuvjv133",
                Make = "Nissan",
                Model = "Qasqhai",
                Mileage = 95000,
                ReleasedYear = 2018,
                LastRevisionYear = 2020,
                TransmissionMode = "AUTOMATIC"
            };

            var mockException = new FormatException("Format error");

            var carRepository = new Mock<ICarRepository>();

            carRepository.Setup(repository => repository.GetCar(It.IsAny<String>()))
                .Returns(existingCar);

            carRepository.Setup(repository => repository.UpdateCarAsync(It.IsAny<Car>()))
                .Throws(mockException);

            //Act
            var controller = new CarsController(carRepository.Object);
            var result = await controller.PutAsync(carmodel);

            //Assert
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public async Task given_a_car_with_id_when_update_is_succesful_then_return_200_statusAsync()
        {
            //Arrange
            var carmodel = new IdentifiedCarModel
            {
                Id = "tfuuvjv133",
                Make = "Nissan",
                Model = "Qasqhai",
                Mileage = 135000,
                ReleasedYear = 2018,
                IsTransmissionAutomatic = true,
                LastRevisionYear = 2020
            };
            var id = "tfuuvjv133";

            var existingCar = new Car
            {
                Id = "tfuuvjv133",
                Make = "Nissan",
                Model = "Qasqhai",
                Mileage = 95000,
                ReleasedYear = 2018,
                LastRevisionYear = 2020,
                TransmissionMode = "AUTOMATIC"
            };


            var carRepository = new Mock<ICarRepository>();


            carRepository.Setup(repository => repository.GetCar(It.IsAny<String>()))
                .Returns(existingCar);


            carRepository.Setup(repository => repository.UpdateCarAsync(It.IsAny<Car>()))
                .Returns(Task.FromResult(id));

            //Act
            var controller = new CarsController(carRepository.Object);
            var result = await controller.PutAsync(carmodel);

            //Assert
            Assert.AreEqual(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public async Task given_a_car_with_id_unknown_in_database_when_update_is_succesful_then_return_201_statusAsync()
        {
            //Arrange
            var carmodel = new IdentifiedCarModel
            {
                Id = "tfuuvjv133",
                Make = "Nissan",
                Model = "Qasqhai",
                Mileage = 135000,
                ReleasedYear = 2018,
                IsTransmissionAutomatic = true,
                LastRevisionYear = 2020
            };
            var id = "tfuuvjv133";

            var carRepository = new Mock<ICarRepository>();

            carRepository.Setup(repository => repository.CreateAsync(It.IsAny<Car>()))
                .Returns(Task.FromResult(id));

            //Act
            var controller = new CarsController(carRepository.Object);
            var result = await controller.PutAsync(carmodel);

            //Assert
            Assert.AreEqual(StatusCodes.Status201Created, ((ObjectResult)result).StatusCode);

        }
    }
}
