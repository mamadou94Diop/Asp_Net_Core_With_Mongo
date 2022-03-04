﻿using System;
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

        /// <summary>
        /// Retrieves a list of cars available in catalog matching optionally filter on releasedYear
        /// </summary>
        /// <returns>list of cars </returns>
        /// <param name="minimalReleasedYear">The minimal released year api should start searching car</param>
        /// <param name="maximalReleasedYear">The maximal released year api should stop searching car beyond</param>
        /// <response code="200">List of cars returned</response>
        /// <response code="500">An error occured from server</response>
        [ProducesResponseType(typeof(List<CarModel>),200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public IActionResult Get([FromQuery] int? minimalReleasedYear, [FromQuery] int? maximalReleasedYear)
        {
            try
            {
                var cars = repository.GetCars(minimalReleasedYear, maximalReleasedYear).ConvertAll(car => car.ToCarModel());
                return Ok(cars);
            } catch(Exception exception)
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
        [ProducesResponseType(typeof(CarModel), 200)]
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
                    return Ok(car.ToCarModel());

                }
                else {
                    return NotFound("This car id is unknown");
                }
            }
            catch(FormatException exception)
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

        /// <summary>
        /// Updates a car in catalog
        /// </summary>
        /// <param name="carModel"></param>
        /// <response code="200">The car is successfully updated</response>
        /// <response code="201">The car is successfully created</response>
        /// <response code="400">Data received is not valid</response>
        /// <response code="500">An error occured from server</response>
        [HttpPut]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        public async Task<IActionResult> PutAsync(CarModel carModel)
        {
            if(carModel.Id == null)
            {
                return await PostAsync(carModel);
            }
            else
            {
                try
                {
                    var updatedCarId = await repository.UpdateCarAsync(carModel.ToCar());
                    return Ok(updatedCarId);
                }
                catch(FormatException exception)
                {
                    return BadRequest(exception.Message);
                }
                catch (Exception exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "an error occured");
                }

            }
            
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
