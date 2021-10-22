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
    public class CarsControllerTest
    {
      
        [Test]
        public async Task given_a_new_car_when_insert_succeeded_then_return_201_statusAsync()
        {
            //Arrange

            var carModel = new CarModel
            {
                Make = "BMW",
                Model = "X6",
                ReleasedYear = 2020,
                IsTransmissionAutomatic = true,
                Mileage = 5000
            };

            var fakeCreatedCarId = Task.FromResult<String>("fhfhflhflhflhf");

            var carRepository = new Mock<ICarRepository>();

            carRepository.Setup (repository => repository.CreateAsync(It.IsAny<Car>()))
                         .Returns(fakeCreatedCarId);


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
            var carModel = new CarModel
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
            carRepository.Setup(repository => repository.GetCars(null, null))
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
    }
}