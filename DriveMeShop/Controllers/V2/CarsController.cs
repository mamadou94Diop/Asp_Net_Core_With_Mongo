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

        /// <summary>
        /// Retrieves all the cars available in catalogue
        /// </summary>
        /// <returns>list of cars </returns>
        /// <response code="200">List of cars returned</response>
        /// <response code="500">An error occured from server</response>
        [ProducesResponseType(typeof(List<IdentifiedCarModel>), 200)]
        [ProducesResponseType(500)]
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

        /// <summary>
        /// Retrieve car with id matching with passed parameter
        /// </summary>
        /// <param name="id">id of the car to search</param>
        /// <returns> the car corresponding with that id</returns>
        /// <response code="200">The car with id passed was searched was found and returned</response>
        /// <response code="404"> The car with id passed was not found</response>>
        /// <response code="500">An error occured from server</response>
        [ProducesResponseType(typeof(IdentifiedCarModel), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
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


        /// <summary>
        /// Creates a new car in catalog
        /// </summary>
        /// <param name="carModel"></param>
        /// <returns>id of new inserted car</returns>
        /// <response code="201">The car is succesfully created</response>
        /// <response code="500">An error occured from the server when creating the car</response>
        /// <response code="400">Data sent is not valid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(500)]
        [ProducesResponseType(400)]
        [Consumes("application/json")]
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
    }
}
