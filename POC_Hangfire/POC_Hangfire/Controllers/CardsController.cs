using Hangfire;
using Microsoft.AspNetCore.Mvc;
using POC_Hangfire.Models;
using POC_Hangfire.Repositories;
using System;
using System.Threading.Tasks;

namespace POC_Hangfire.Controllers
{
    [ApiController()]
    [Route("[controller]")]
    public class CardsController : ControllerBase
    {
        private static int _count = 0;
        private readonly ICarRepository _carRepository;

        public CardsController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }


        [HttpPost("")]
        public IActionResult AddCar()
        {
            ++_count;
            var car = new Car { Description = "Gol ", Fabricator = "VW", Id = _count, Model = "Gol", Motor = "2.0" };

            BackgroundJob.Schedule<ICarRepository>(x  => x.SaveAsync(car), TimeSpan.FromSeconds(5));

            return Created("", _count);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _carRepository.GetAsync(id);
            if (car is null)
                return NotFound();

            return Ok(car);
        }
    }
}
