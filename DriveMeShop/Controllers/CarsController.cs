using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using DriveMeShop.Entity;
using DriveMeShop.Extension;
using DriveMeShop.Model;
using DriveMeShop.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DriveMeShop.Controllers
{
    [ApiVersion("1.0")]
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
        /// Retrieves a list of cars available in catalog, matching optionnally with some filters
        /// </summary>
        /// <returns>list of cars </returns>
        /// <param name="minimalReleasedYear">The year API should shart look up cars released that year and beyond</param>
        /// <param name="maximalReleasedYear">the last year API should look up cars released to that year</param>
        /// <response code="200">List of cars returned</response>
        /// <response code="500">An error occured from server</response>
        [ProducesResponseType(typeof(List<IdentifiedCarModel>), 200)]
        [ProducesResponseType(500)]
        [HttpGet]
        public IActionResult Get([FromQuery] int? minimalReleasedYear, [FromQuery] int? maximalReleasedYear)
        {
            try
            {
                var cars = repository.GetCars(minimalReleasedYear, maximalReleasedYear);

                var identifiedCarsModel = mapper.Map<List<Car>, List<IdentifiedCarModel>> (cars);

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
                Car car = mapper.Map<UnidentifiedCarModel, Car>(carModel);
                var newCarId = await repository.CreateAsync(car);

                return Created("", newCarId);

            }
            catch (Exception exception)
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
        public async Task<IActionResult> PutAsync(IdentifiedCarModel carModel)
        {
            if (carModel.Id == null)
            {
                return await PostAsync(carModel);
            }
            else
            {
                try
                {
                    var car = repository.GetCar(carModel.Id);
                    if (car != null)
                    {
                        Car updatedCar = mapper.Map <IdentifiedCarModel, Car>(carModel) ;
                        var updatedCarId = await repository.UpdateCarAsync(updatedCar);
                        return Ok(updatedCarId);
                    }
                    else
                    {
                        return await PostAsync(carModel);
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

        }
        /// <summary>
        /// Updates revision year field
        /// </summary>
        /// <param name="id">Id of the car the revision year has to be updated.</param>
        /// <param name="lastRevisionYearModel">the new value of revision year to introduce.</param>
        /// <returns>No content with acknowledgement of update in case of success</returns>
        /// <response code="204">The update was successful.</response>
        /// <response code="400">The data received is invalid or may cause inconsistency issue.</response>
        /// <response code="404">No car with that received id was found.</response>
        /// <response code="500">An error occured from server/</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        public async Task<IActionResult> PatchAsync(string id, CarLastRevisionYearModel lastRevisionYearModel)
        {
            try
            { 
                var car = repository.GetCar(id);
                if(car != null)
                {
                    var isValid = (lastRevisionYearModel.LastRevisionYear == null) || (car.ReleasedYear <= lastRevisionYearModel.LastRevisionYear);

                    if (isValid)
                    {
                        var result = await repository.UpdateCarLastRevisionYearAsync(id, lastRevisionYearModel.LastRevisionYear);
                        if(result != null)
                        {
                            return NoContent();
                        }
                        else

                        {
                            return StatusCode(StatusCodes.Status500InternalServerError, "The update was not successful");
                        }

                    } else
                    {
                        var message = "Car released year should not be greater than last revision year";

                        return BadRequest(message);
                    }
                }
                else
                {
                    var message = $"Car with id {id} not found";
                    return NotFound(message);
                }

            }
            catch (FormatException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "an error occured");

            }

        }

        /// <summary>
        /// Deletes a car from the catalog
        /// </summary>
        /// <param name="id">the id of the car to be deleted.</param>
        /// <returns>No content with acknowledgement of success.</returns>
        /// <response code="204">The deletion was successful.</response>
        /// <response code="404">The car with the id received was not found.</response>
        /// <response code="400">The format of the id received is not good.</response>
        /// <response code="500">An error occured during the deletion.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                var car = repository.GetCar(id);

                if(car != null)
                {
                    var isDeleteSuccesful = await repository.DeleteCarAsync(id);

                    if (isDeleteSuccesful)
                    {
                        return NoContent();
                    } else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Delete operation was not successful.");
                    }
                }
                else
                {
                    var message = $"Car with id {id} not found";
                    return NotFound(message);

                }
            }
            catch (FormatException exception)
            {
                return BadRequest(exception.Message);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "an error occured");

            }
        }


    }
}
