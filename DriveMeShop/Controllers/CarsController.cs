using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using DriveMeShop.Extension;
using DriveMeShop.Model;
using DriveMeShop.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DriveMeShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : Controller
    {
        private readonly ICarRepository repository;

        public CarsController(ICarRepository _repository)
        {
            repository = _repository;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return ""+id;
        }

        /// <summary>
        /// Creates a new car in catalog
        /// </summary>
        /// <param name="carModel"></param>
        /// <returns>id of new inserted car</returns>
        /// <response code="201">The car is succesfully created</response>
        /// <response code="500">An error occure from the server when creating the car</response>
        /// <response code="400">Data sent is not valid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(string),201)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [Consumes("application/json")]
        public async Task<IActionResult> PostAsync(CarModel carModel)
        {
            try
            {
                var newCarId = await repository.CreateAsync(carModel.ToCar());

                return Created("", newCarId);

            } catch(Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "an error occured");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
