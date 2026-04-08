using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    [ApiController]
    [Route("api/rentals")]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalsService _rentalsService;

        public RentalsController(IRentalsService rentalsService)
        {
            _rentalsService = rentalsService;
        }
        [HttpGet]
        public async Task<IEnumerable<Rental>> Get()
        {
            return await _rentalsService.GetAllActiveRentals();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> Get(Guid id)
        {
            return await _rentalsService.GetRental(id);
        }

        [HttpPost]
        public async Task<ActionResult<Rental>> Post([FromBody] Rental model)
        {
            return Ok(await _rentalsService.Create(model));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Rental>> Put(Guid id, [FromBody] Rental model)
        {
            if (model.Id != id)
                return BadRequest();

            return Ok(await _rentalsService.Update(model));
        }
        [HttpPut("{id}/start")]
        public async Task<ActionResult<Rental>> StartRental(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest();

            return Ok(await _rentalsService.StartRental(id));
        }
        [HttpPut("{id}/finish")]
        public async Task<ActionResult<Rental>> FinishRental(Guid id, [FromBody] decimal milageInKm)
        {
            if (id == Guid.Empty)
                return BadRequest();

            return Ok(await _rentalsService.FinishRental(id, milageInKm));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Rental>> Delete(Guid id)
        {
            return Ok(await _rentalsService.Delete(id));
        }
    }
}
