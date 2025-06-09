using Microsoft.AspNetCore.Authorization; // Бібліотека для авторизації
using Microsoft.AspNetCore.Mvc; // Бібліотека для створення API-контролерів

namespace UMP_API.Controllers // Простір імен для контролерів
{
    // Атрибут, який позначає клас як API-контролер
    [ApiController]
    // Атрибут, який визначає маршрут для всіх методів контролера
    [Route("api/[controller]")]
    public class MarketController : ControllerBase // Базовий клас для API-контролерів
    {
        // Атрибут, який вимагає авторизацію для доступу до методу
        [HttpGet]
        [Authorize]
        public IActionResult GetItems()
        {
            // Повертаємо статичний список товарів (поки що для тесту)
            var items = new[] { "App1", "App2", "App3" };
            return Ok(items); // Повертаємо 200 OK із даними
        }
    }
}