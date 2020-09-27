using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EFCoreMEmoryLeak.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AppDbContext _context;
        private readonly IServiceProvider _provider;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext context, IServiceProvider provider)
        {
            _logger = logger;
            _context = context;
            _provider = provider;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            _context.Entities.Add(new Entity { Name = "Bar" });
            await _context.SaveChangesAsync();

            // Uncomment this and compare memory footprint after GC
            //_context.ChangeTracker.Entries().ToList().ForEach(_ => _.State = EntityState.Detached);
            // Just to be sure GC run
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                GC.Collect(2, GCCollectionMode.Forced, blocking: true, compacting: true);
            });

            return result;
        }
    }
}
