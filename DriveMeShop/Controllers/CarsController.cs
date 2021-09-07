using System;
using System.Collections.Generic;
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

        // POST api/values
        [HttpPost]
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
