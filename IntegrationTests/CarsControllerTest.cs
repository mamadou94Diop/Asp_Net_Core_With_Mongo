using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DriveMeShop.Entity;
using DriveMeShop.Model.V1;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTests
{
    public partial class CarsControllerTests
    {
        private HttpClient httpClient;
        private TestApplicationFactory testApplicationFactory;


        [SetUp]
        public void Setup()
        {
            testApplicationFactory = new TestApplicationFactory();
            httpClient = testApplicationFactory.CreateClient(); 
        }

        [Test]
        public async Task given_a_car_when_insert_is_succesful_then_return_201_status()
        {
            //Arrange
            var car = new UnidentifiedCarModel
            {
                Make = "Toyota",
                Model = "Yaris",
                Mileage = 45000,
                ReleasedYear = 2018,
                LastRevisionYear = 2020,
                IsTransmissionAutomatic = true

            };

            var jsonBody = JsonConvert.SerializeObject(car);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PostAsync("/api/v1/cars", body);


            //Assert
            Assert.AreEqual(true, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        }

        [Test]
        public async Task given_a_car_with_invalid_information_when_insert_then_return_400_status()
        {
            //Arrange
            var car = new UnidentifiedCarModel
            {
                Make = "Toyota",
                Model = "Yaris",
                Mileage = -45000,
                ReleasedYear = 2018,
                LastRevisionYear = 2020,
                IsTransmissionAutomatic = true

            };

            var jsonBody = JsonConvert.SerializeObject(car);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PostAsync("/api/v1/cars", body);

            //Assert
            Assert.AreEqual(false, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task given_a_car_with_inconsistent_information_when_insert_then_return_400_status()
        {
            //Arrange
            var car = new UnidentifiedCarModel
            {
                Make = "Toyota",
                Model = "Yaris",
                Mileage = 45000,
                ReleasedYear = 2020,
                LastRevisionYear = 2015,
                IsTransmissionAutomatic = true

            };

            var jsonBody = JsonConvert.SerializeObject(car);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PostAsync("/api/v1/cars", body);

            //Assert
            Assert.AreEqual(false, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task given_a_car_with_unknown_id_in_the_database_when_creating_is_successful_then_return_201_status()
        {
            //Arrange
            var car = new IdentifiedCarModel
            {
                Id = "61f667667cb328c6005c8392",
                Make = "Ford",
                Model = "MustangV",
                Mileage = 236700,
                ReleasedYear = 2007,
                LastRevisionYear = 2012,
                IsTransmissionAutomatic = true
            };

            var jsonBody = JsonConvert.SerializeObject(car);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PostAsync("/api/v1/cars", body);
            string jsonResult = await result.Content.ReadAsStringAsync();

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreNotEqual(jsonResult, car.Id);

        }

        [Test]
        public async Task given_a_catalog_when_fetching_cars_is_successful_then_return_200_statusAsync()
        {
            //Act
            var result = await httpClient.GetAsync("/api/v1/cars");

            //Assert
            Assert.AreEqual(true, result.IsSuccessStatusCode);

        }

        [Test]
        public async Task given_a_catalog_when_fetching_car_with_filters_not_matching_on_db_then_return_200_status_with_empty_list()
        {
            //Act
            var result = await httpClient.GetAsync("/api/v1/cars?minimalReleasedYear=2017&maximalReleasedYear=2021");

            var jsonResult = await result.Content.ReadAsStringAsync();
            var cars = JsonConvert.DeserializeObject<List<Car>>(jsonResult);

            //Assert
            Assert.AreEqual(true, result.IsSuccessStatusCode);
            Assert.AreEqual(0, cars.Count);
        }
        [Test]
        public async Task given_an_id_when_fetching_car_with_that_id_is_successful_then_return_200_status()
        {
            //Act
            var result = await httpClient.GetAsync("/api/v1/cars/6168c06d89af83d580f6e01e");

            //Assert
            Assert.AreEqual(true, result.IsSuccessStatusCode);

        }

        [Test]
        public async Task given_an_id_when_fetching_car_with_that_id_returns_null_then_return_404_status()
        {
            //Act
            var result = await httpClient.GetAsync("/api/v1/cars/6168cb8d6abab7b8855aa5a0");

            //Assert
            Assert.AreEqual(false, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Test]
        public async Task given_an_invalid_id_when_trying_to_fetch_car_with_that_id_then_return_400_status()
        {
            //Act
            var result = await httpClient.GetAsync("/api/v1/cars/6168cb8d6ab7b8855aa5a0");

            //Assert
            Assert.AreEqual(false, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task given_a_car_without_id_when_updating_is_successful_then_return_201_status()
        {
            //Arrange
            var car = new IdentifiedCarModel {
                Make = "Ford",
                Model = "MustangV",
                Mileage = 236700,
                ReleasedYear = 2007,
                LastRevisionYear = 2012,
                IsTransmissionAutomatic = true
            };

            var jsonBody = JsonConvert.SerializeObject(car);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PutAsync("/api/v1/cars", body);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

        }

        [Test]
        public async Task given_a_car_with_unknown_id_in_the_database_when_updating_is_successful_then_return_201_status()
        {
            //Arrange
            var car = new IdentifiedCarModel
            {
                Id = "61f667667cb328c6005c8392",
                Make = "Ford",
                Model = "MustangV",
                Mileage = 236700,
                ReleasedYear = 2007,
                LastRevisionYear = 2012,
                IsTransmissionAutomatic = true
            };

            var jsonBody = JsonConvert.SerializeObject(car);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PutAsync("/api/v1/cars", body);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

        }

        [Test]
        public async Task given_a_car_with_id_when_update_is_successful_then_return_200_statusAsync()
        {
            //Arrange
            var car = new IdentifiedCarModel
            {
                Id = "6168c06d89af83d580f6e01e",
                Make = "BMW",
                Model = "X6",
                IsTransmissionAutomatic = true,
                ReleasedYear = 2016,
                LastRevisionYear = 2020,
                Mileage = 120000
            };

            var jsonBody = JsonConvert.SerializeObject(car);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PutAsync("/api/v1/cars", body);

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            httpClient.Dispose();
            testApplicationFactory.Dispose();
        }
    }
}
