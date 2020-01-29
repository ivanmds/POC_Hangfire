using Microsoft.AspNetCore.Mvc;
using POC_Hangfire.Models;
using POC_Hangfire.Repositories;
using System;
using System.Threading.Tasks;

namespace POC_Hangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarRepository _repository;

        public CarsController(ICarRepository repository)
            => _repository = repository;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCard(string id)
        {
            return Ok(await _repository.GetAsync(id));
        }

        [HttpPost("")]
        public async Task<IActionResult> SaveCard([FromBody]Car car)
        {
            car.Id = Guid.NewGuid().ToString();
            await _repository.SaveAsync(car);
            return Ok(car);
        }
    }
}