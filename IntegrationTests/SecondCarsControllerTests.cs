using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DriveMeShop.Model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace IntegrationTests
{
    public partial class CarsControllerTests
    {
       [Test]
       public async Task given_a_valid_car_id_when_updating_last_revision_year_with_valid_value_then_return_204()
        {
            //Arrange
            var lastRevisionYearModel = new CarLastRevisionYearModel
            {
                LastRevisionYear = 2021
            };

            var jsonBody = JsonConvert.SerializeObject(lastRevisionYearModel);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PatchAsync("api/cars/6168c06d89af83d580f6e01e", body);

            //Assert
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Test]
        public async Task given_a_valid_car_id_when_updating_last_revision_year_with_invalid_value_then_return_400()
        {
            //Arrange
            var lastRevisionYearModel = new CarLastRevisionYearModel
            {
                LastRevisionYear = 2015
            };

            var jsonBody = JsonConvert.SerializeObject(lastRevisionYearModel);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PatchAsync("api/cars/6168c06d89af83d580f6e01e", body);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task given_an_invalid_car_id_when_updating_last_revision_with_any_value_then_return_400()
        {
            //Arrange
            var lastRevisionYearModel = new CarLastRevisionYearModel
            {
                LastRevisionYear = 2020
            };

            var jsonBody = JsonConvert.SerializeObject(lastRevisionYearModel);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PatchAsync("api/cars/6168c06d89af8380f6e01e", body);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public async Task given_an_unknown_car_id_when_trying_to_update_last_revision_year_with_any_value_then_return_404()
        {
            //Arrange
            var lastRevisionYearModel = new CarLastRevisionYearModel
            {
                LastRevisionYear = 2021
            };

            var jsonBody = JsonConvert.SerializeObject(lastRevisionYearModel);
            var body = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            //Act
            var result = await httpClient.PatchAsync("api/cars/61f667667cb328c6005c8392", body);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
