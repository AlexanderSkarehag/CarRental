using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarsController : ControllerBase
    {
        private readonly ICarsService _carsService;

        public CarsController(ICarsService carsService)
        {
            _carsService = carsService;
        }
        [HttpGet]
        public async Task<IEnumerable<Car>> Get()
        {
            return await _carsService.GetAllCars();
        }
        [HttpGet("available")]
        public async Task<IEnumerable<Car>> GetAvailable()
        {
            return await _carsService.GetAvailableCars();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> Get(Guid id)
        {
            return await _carsService.GetCar(id);
        }

        [HttpPost]
        public async Task<ActionResult<Car>> Post([FromBody] Car model)
        {
            return Ok(await _carsService.Create(model));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Car>> Put(Guid id, [FromBody] Car model)
        {
            if (model.Id != id)
                return BadRequest();

            return Ok(await _carsService.Update(model));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Car>> Delete(Guid id)
        {
            return Ok(await _carsService.Delete(id));
        }
    }
}
