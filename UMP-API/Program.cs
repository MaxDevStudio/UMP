using System.Text; // Бібліотека для роботи з текстом
using Microsoft.AspNetCore.Authentication.JwtBearer; // Бібліотека для JWT-авторизації
using Microsoft.EntityFrameworkCore; // Бібліотека для роботи з базою даних через EF Core
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens; // Бібліотека для створення і перевірки JWT
using UMP_API.Data; // Простір імен для контексту бази даних

var builder = WebApplication.CreateBuilder(args);

// Реєструємо сервіси для контролерів (щоб обробляти HTTP-запити)
builder.Services.AddControllers();

// Додаємо Swagger для зручного тестування API в режимі розробки
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Налаштовуємо підключення до PostgreSQL
builder.Services.AddDbContext<UmpWinDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); // Використовуємо PostgreSQL із конфігурації
});

// Налаштовуємо JWT-авторизацію
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt"); // Отримуємо налаштування JWT із appsettings.json
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // Перевіряємо, чи валідний видавець токена
            ValidateAudience = true, // Перевіряємо, чи валідна аудиторія токена
            ValidateLifetime = true, // Перевіряємо термін дії токена
            ValidateIssuerSigningKey = true, // Перевіряємо підпис токена
            ValidIssuer = jwtSettings["Issuer"], // Встановлюємо видавача токена
            ValidAudience = jwtSettings["Audience"], // Встановлюємо аудиторію токена
            IssuerSigningKey = new // Секретний ключ для підпису
            SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)) // Додано !
         };
    });

var app = builder.Build();

// Увімкнення Swagger у режимі розробки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Налаштовуємо перенаправлення на HTTPS для безпеки
app.UseHttpsRedirection();

// Додаємо middleware для авторизації
app.UseAuthentication();
app.UseAuthorization();

// Налаштовуємо маршрутизацію для контролерів
app.MapControllers();

// Ініціалізація бази даних і додавання тестового користувача
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UmpWinDbContext>(); // Отримуємо контекст бази
    db.Database.EnsureCreated(); // Створюємо базу, якщо її немає
    if (!db.Users.Any()) // Перевіряємо, чи є користувачі
    {
        db.Users.Add(new User // Додаємо тестового користувача
        {
            Username = "testuser", // Ім'я користувача
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("testpass"), // Хеш пароля
            Role = "admin" // Роль користувача
        });
        db.SaveChanges(); // Зберігаємо зміни в базі
    }
}

// Запускаємо додаток
app.Run();