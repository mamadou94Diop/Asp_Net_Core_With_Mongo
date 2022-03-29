using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DriveMeShop.Entity;
using DriveMeShop.Model.V2;
using DriveMeShop.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DriveMeShop.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CarsController : Controller
    {
        private readonly ICarRepository repository;
        private readonly IMapper mapper;


        public CarsController(ICarRepository _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var cars = repository.GetCars(null, null);

                var identifiedCarsModel = mapper.Map<List<Car>, List<IdentifiedCarModel>>(cars);

                return Ok(identifiedCarsModel);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "an error occured");
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                var car = repository.GetCar(id);
                if (car != null)
                {
                    IdentifiedCarModel identifiedCar = mapper.Map<Car, IdentifiedCarModel>(car);
                    return base.Ok(identifiedCar);

                }
                else
                {
                    return NotFound("This car id is unknown");
                }
            }
            catch (FormatException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "an error occured");

            }
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(UnidentifiedCarModel carModel)
        {
            try
            {
                var car = mapper.Map<Car>(carModel);
                var newCarId = await repository.CreateAsync(car);
                return Created("", newCarId);
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "an error occured");

            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
