using Microsoft.EntityFrameworkCore; // Бібліотека для роботи з базою даних

namespace UMP_API.Data // Простір імен для організації коду
{
    // Клас контексту бази даних
    public class UmpWinDbContext : DbContext
    {
        // Властивість для доступу до таблиці Users
        public DbSet<User> Users { get; set; }

        // Конструктор, який приймає опції для підключення
        public UmpWinDbContext(DbContextOptions<UmpWinDbContext> options) : base(options)
        {
        }
    }

    // Клас, що представляє користувача в базі
    public class User
    {
        public int Id { get; set; } // Унікальний ідентифікатор (автоінкремент)
        public required string Username { get; set; } // Ім'я користувача
        public required string PasswordHash { get; set; } // Хеш пароля
        public required string Role { get; set; } // Роль користувача
    }
}