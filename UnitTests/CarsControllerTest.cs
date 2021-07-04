using System;
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
    }
}