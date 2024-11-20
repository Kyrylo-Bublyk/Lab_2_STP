using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MySwaggerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[] { 
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" 
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Получить список прогнозов погоды.
        /// </summary>
        /// <returns>Список объектов WeatherForecast</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), 200)]  // Успешный ответ с кодом 200
        [ProducesResponseType(500)]  // Ошибка сервера с кодом 500
        public IEnumerable<WeatherForecast> Get()
        {
            try
            {
                _logger.LogInformation("Запрос на прогноз погоды принят");

                var rng = new Random();
                var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                }).ToArray();

                _logger.LogInformation("Прогноз погоды успешно сгенерирован");

                return forecasts;
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                _logger.LogError($"Произошла ошибка: {ex.Message}\n{ex.StackTrace}");

                // Возвращаем код 500 и пустой список данных
                Response.StatusCode = 500;
                return new List<WeatherForecast>();
            }
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string? Summary { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }
}
