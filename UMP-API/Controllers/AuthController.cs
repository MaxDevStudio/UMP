using Microsoft.AspNetCore.Mvc; // Бібліотека для створення API-контролерів
using Microsoft.IdentityModel.Tokens; // Бібліотека для роботи з JWT
using System.IdentityModel.Tokens.Jwt; // Бібліотека для створення JWT-токенів
using System.Security.Claims; // Бібліотека для роботи з claims у JWT
using System.Text; // Бібліотека для роботи з текстом
using UMP_API.Data; // Простір імен для контексту бази

namespace UMP_API.Controllers // Простір імен для контролерів
{
    // Атрибут, який позначає клас як API-контролер
    [ApiController]
    // Атрибут, який визначає маршрут для всіх методів контролера
    [Route("api/[controller]")]
    public class AuthController : ControllerBase // Базовий клас для контролерів
    {
        private readonly UmpWinDbContext _context; // Контекст бази даних
        private readonly IConfiguration _configuration; // Конфігурація для доступу до appsettings.json

        // Конструктор для ін'єкції залежностей
        public AuthController(UmpWinDbContext context, IConfiguration configuration)
        {
            _context = context; // Зберігаємо контекст
            _configuration = configuration; // Зберігаємо конфігурацію
        }

        // Метод для логіну (POST-запит)
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Шукаємо користувача в базі за ім'ям
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            // Перевіряємо, чи користувач існує і чи правильний пароль
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized("Неправильний логін або пароль"); // Повертаємо 401, якщо помилка
            }

            // Отримуємо налаштування JWT із конфігурації
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // Налаштовуємо підпис

            // Створюємо JWT-токен
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"], // Видавець токена
                audience: jwtSettings["Audience"], // Аудиторія токена
                claims: new[] // Додаємо claims (дані про користувача)
                {
                    new Claim("userId", user.Id.ToString()),
                    new Claim("role", user.Role)
                },
                expires: DateTime.Now.AddHours(1), // Термін дії токена (1 година)
                signingCredentials: creds // Підпис токена
            );

            // Повертаємо токен у відповідь
            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }

    // Клас для десеріалізації тіла запиту логіну
    public class LoginRequest
    {
        public required string Username { get; set; } // Ім'я користувача
        public required string Password { get; set; } // Пароль
    }
}