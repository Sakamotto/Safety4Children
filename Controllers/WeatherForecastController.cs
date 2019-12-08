using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Safety4Children.Entities;
using Safety4Children.Repository;

namespace Safety4Children.Controllers
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
        private readonly AppDbContext Db;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext context)
        {
            _logger = logger;
            Db = context;
        }

        [HttpGet]
        public IEnumerable<UsuarioPai> Get()
        {
            var usuariosPai = Db.Set<UsuarioPai>().ToArray();
            return usuariosPai;
        }
    }
}
