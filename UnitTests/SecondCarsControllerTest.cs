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
    partial class CarsControllerTest
    {
        [Test]
        public async Task given_a_car_with_valid_id_when_updating_last_revision_year_with_valid_value_succeeded_then_return_204()
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

            var carId = "112TVTVTVT";

            var lastRevisionYearModel = new CarLastRevisionYearModel
            {
                LastRevisionYear = 2021
            };

            var mockCarRepository = new Mock<ICarRepository>();

            mockCarRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Returns(mockCar);

            mockCarRepository.Setup(repository => repository.UpdateCarLastRevisionYearAsync(It.IsAny<string>(),It.IsAny<int?>()))
                .Returns(Task.FromResult(carId));

            //Act

            var controller = new CarsController(mockCarRepository.Object);
            var result = await controller.PatchAsync(carId, lastRevisionYearModel);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task given_a_car_with_valid_id_when_updating_last_revision_year_with_valid_value_failed_then_return_500()
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

            var carId = "112TVTVTVT";

            var lastRevisionYearModel = new CarLastRevisionYearModel
            {
                LastRevisionYear = 2021
            };

            var mockCarRepository = new Mock<ICarRepository>();

            mockCarRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Returns(mockCar);

            mockCarRepository.Setup(repository => repository.UpdateCarLastRevisionYearAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .Returns(Task.FromResult<string>(null));

            //Act

            var controller = new CarsController(mockCarRepository.Object);
            var result = await controller.PatchAsync(carId, lastRevisionYearModel);

            //Assert
            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);

        }

        [Test]
        public async Task given_a_car_with_valid_id_when_updating_last_revision_year_with_invalid_value_then_return_400()
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

            var carId = "112TVTVTVT";

            var lastRevisionYearModel = new CarLastRevisionYearModel
            {
                LastRevisionYear = 2019
            };


            var mockCarRepository = new Mock<ICarRepository>();

            mockCarRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Returns(mockCar);

            //Act

            var controller = new CarsController(mockCarRepository.Object);
            var result = await controller.PatchAsync(carId, lastRevisionYearModel);

            //Assert

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task given_a_car_which_does_not_exist_when_updating_last_revision_year_with_any_value_then_return_404()
        {
            //Arrange
            var carId = "112TVTVTVT";

            var lastRevisionYearModel = new CarLastRevisionYearModel
            {
                LastRevisionYear = 2019
            };

            Car mockCar = null;

            var mockCarRepository = new Mock<ICarRepository>();

            mockCarRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Returns(mockCar);

            //Act

            var controller = new CarsController(mockCarRepository.Object);
            var result = await controller.PatchAsync(carId, lastRevisionYearModel);

            //Assert

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task given_a_car_with_valid_id_when_updating_last_revision_year_throws_exception_then_return_500()
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

            var carId = "112TVTVTVT";

            var mockException = new Exception("failure during update");

            var lastRevisionYearModel = new CarLastRevisionYearModel
            {
                LastRevisionYear = 2021
            };

            var mockCarRepository = new Mock<ICarRepository>();

            mockCarRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Returns(mockCar);

            mockCarRepository.Setup(repository => repository.UpdateCarLastRevisionYearAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .Throws(mockException);

            //Act

            var controller = new CarsController(mockCarRepository.Object);
            var result = await controller.PatchAsync(carId, lastRevisionYearModel);

            //Assert

            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
        }

        [Test]
        public async Task given_a_car_id_when_deletion_of_a_car_with_that_id_is_successful_then_return_204()
        {
            //Arrange

            var carId = "173829UDJDJ";
            var mockCar = new Car();

            var mockCarRepository = new Mock<ICarRepository>();

            mockCarRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Returns(mockCar);

            mockCarRepository.Setup(repository => repository.DeleteCarAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            //Act

            var controller = new CarsController(mockCarRepository.Object);
            var result = await controller.DeleteAsync(carId);

            //Assert

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task given_a_car_id_when_deletion_of_a_car_with_that_id_is_not_successful_then_return_500()
        {
            //Arrange

            var carId = "173829UDJDJ";
            var mockCar = new Car();

            var mockCarRepository = new Mock<ICarRepository>();

            mockCarRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Returns(mockCar);

            mockCarRepository.Setup(repository => repository.DeleteCarAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            //Act

            var controller = new CarsController(mockCarRepository.Object);
            var result = await controller.DeleteAsync(carId);

            //Assert

            Assert.AreEqual(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);

        }

        [Test]
        public async Task given_a_car_id_when_a_car_with_that_id_is_not_found_then_return_404()
        {
            //Arrange
            var carId = "33939";

            Car mockCar = null;

            var mockCarRepository = new Mock<ICarRepository>();

            mockCarRepository.Setup(repository => repository.GetCar(It.IsAny<string>()))
                .Returns(mockCar);

            //Act

            var controller = new CarsController(mockCarRepository.Object);
            var result = await controller.DeleteAsync(carId);

            //Assert

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }
    }
}