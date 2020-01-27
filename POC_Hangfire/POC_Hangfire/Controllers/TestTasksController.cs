using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace POC_Hangfire.Controllers
{
    [ApiController()]
    [Route("[controller]")]
    public class TestTasksController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobs;
        private static int _count = 0;

        public TestTasksController(IBackgroundJobClient backgroundJobs)
            => _backgroundJobs = backgroundJobs;

        [HttpGet("")]
        public void AddTask()
        {
            ++_count;
            _backgroundJobs.Enqueue(() => Console.WriteLine($"Count {_count}", TimeSpan.FromSeconds(1)));
        }
    }
}
