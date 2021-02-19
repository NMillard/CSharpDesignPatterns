using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GlobalMiddlewarePipeline.Controllers {
    
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger) {
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Bad() {
            // logic whatever
            // if fails
            throw new NotGoodException("damn son", ErrorCode.Bad);
        }
        
        [HttpGet("Catastrophic")]
        public IActionResult Catastrophic() {
            throw new NotGoodException("exceptional stuff", ErrorCode.Catastrophic);
        }
        
        [HttpGet("TooBusy")]
        public IActionResult TooBusy() {
            throw new TooBusyException("way too busy", ErrorCode.VeryBad, DateTime.Now.AddSeconds(30));
        }

        [HttpGet("Invalid")]
        public IActionResult Invalid() {
            throw new InvalidOperationException();
        }
    }
}