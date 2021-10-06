using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DriveMeShop.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTests
{
    public class CarsControllerTests
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
            var car = new CarModel
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
            var result = await httpClient.PostAsync("/api/cars", body);


            //Assert
            Assert.AreEqual(true, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        }

        [Test]
        public async Task given_a_car_with_invalid_information_when_insert_then_return_400_status()
        {
            //Arrange
            var car = new CarModel
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
            var result = await httpClient.PostAsync("/api/cars", body);

            //Assert
            Assert.AreEqual(false, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task given_a_car_with_inconsistent_information_when_insert_then_return_400_status()
        {
            //Arrange
            var car = new CarModel
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
            var result = await httpClient.PostAsync("/api/cars", body);

            //Assert
            Console.WriteLine(result.ReasonPhrase);
            Assert.AreEqual(false, result.IsSuccessStatusCode);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TearDown]
        public void TearDown()
        {
            httpClient.Dispose();
            testApplicationFactory.Dispose();
        }
    }
}
