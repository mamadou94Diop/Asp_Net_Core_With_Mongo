using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DriveMeShop.Model.V2;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTests
{
    public class CarsControllerV2Test
    {
        private  HttpClient httpClient;
        private TestApplicationFactory testApplicationFactory;

        [SetUp]
        public void Setup()
        {
            testApplicationFactory = new TestApplicationFactory();
            httpClient = testApplicationFactory.CreateClient();
        }

        [Test]
        public async Task given_a_car_with_v1_format_when_creating_it_with_v2_then_return_bad_request()
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
            var result = await httpClient.PostAsync("api/v2/cars", body);

            //Assert
            Assert.AreEqual(false, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);

        }

        [Test]
        public async Task given_a_car_with_v2_format_when_creating_it_with_v2_then_return_created()
        {
            //Arrange
            var car = new UnidentifiedCarModel
            {
                Make = "Toyota",
                Model = "Yaris",
                Mileage = 45000,
                ReleasedYear = 2018,
                LastRevisionYear = 2020,
                IsTransmissionAutomatic = true,
                MaxSpeed = 200,
                Color= "red"

            };
            var jsonBody = JsonConvert.SerializeObject(car);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");


            //Act
            var result = await httpClient.PostAsync("api/v2/cars", body);

            //Assert
            Assert.AreEqual(true, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

        }

        [TearDown]
        public void TearDown()
        {
            httpClient.Dispose();
            testApplicationFactory.Dispose();
        }
    }
}
