using System.CommandLine; // Бібліотека для створення командного рядка
using System.Net.Http; // Бібліотека для HTTP-запитів
using System.Text; // Бібліотека для роботи з текстом
using System.Text.Json; // Бібліотека для роботи з JSON
using System.Threading.Tasks; // Бібліотека для асинхронних операцій
using System.IO; // Бібліотека для роботи з файлами

class Program
{
    static async Task<int> Main(string[] args) // Головна точка входу
    {
        // Створюємо кореневу команду для CLI
        var rootCommand = new RootCommand("UMP CLI");

        // Команда для логіну
        var loginCommand = new Command("login", "Логін у систему");
        var usernameOption = new Option<string>("--username", "Ім'я користувача") { IsRequired = true }; // Опція для імені
        var passwordOption = new Option<string>("--password", "Пароль") { IsRequired = true }; // Опція для пароля
        loginCommand.AddOption(usernameOption);
        loginCommand.AddOption(passwordOption);
        loginCommand.SetHandler(async (username, password) =>
        {
            await LoginAsync(username, password); // Викликаємо функцію логіну
        }, usernameOption, passwordOption);

        // Команда для роботи з маркетплейсом
        var marketCommand = new Command("market", "Market commands");
        var listCommand = new Command("list", "List items");
        listCommand.SetHandler(async () =>
        {
            await GetMarketItemsAsync(); // Викликаємо функцію для списку
        });
        marketCommand.AddCommand(listCommand);

        // Додаємо команди до кореневої
        rootCommand.AddCommand(loginCommand);
        rootCommand.AddCommand(marketCommand);

        // Виконуємо команду
        return await rootCommand.InvokeAsync(args);
    }

    static async Task LoginAsync(string username, string password) // Функція для логіну
    {
        using var client = new HttpClient(); // Створюємо HTTP-клієнт
        var requestBody = new { username, password }; // Тіло запиту
        var content = new StringContent(
            JsonSerializer.Serialize(requestBody), // Серіалізуємо в JSON
            Encoding.UTF8,
            "application/json");

        try
        {
            var response = await client.PostAsync("http://localhost:5220/api/auth/login", content); // Відправляємо запит
            var responseContent = await response.Content.ReadAsStringAsync(); // Читаємо повну відповідь

            if (response.IsSuccessStatusCode)
            {
                var responseData = JsonSerializer.Deserialize<LoginResponse>(responseContent); // Десеріалізуємо відповідь
                if (string.IsNullOrEmpty(responseData?.token)) // Перевірка токена
                {
                    Console.WriteLine($"Помилка: Токен відсутній або порожній. Відповідь сервера: {responseContent}");
                    return;
                }

                var configDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ump", "config.json"); // Шлях до файлу
                Directory.CreateDirectory(Path.GetDirectoryName(configDir) ?? ""); // Створюємо директорію, якщо її немає
                await File.WriteAllTextAsync(configDir, JsonSerializer.Serialize(new { token = responseData.token })); // Зберігаємо токен
                Console.WriteLine("Логін успішний!");
            }
            else
            {
                Console.WriteLine($"Помилка: {response.StatusCode} - {responseContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }

    static async Task GetMarketItemsAsync() // Функція для отримання списку
    {
        var configPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".ump", "config.json");
        if (!File.Exists(configPath))
        {
            Console.WriteLine("Спочатку виконай логін!");
            return;
        }

        var configJson = await File.ReadAllTextAsync(configPath); // Читаємо токен
        var config = JsonSerializer.Deserialize<Config>(configJson);

        if (string.IsNullOrEmpty(config?.token)) // Змінено на "token" замість "Token"
        {
            Console.WriteLine("Помилка: Токен у config.json відсутній або порожній. Виконай логін повторно.");
            return;
        }

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.token); // Використовуємо "token"

        try
        {
            var response = await client.GetAsync("http://localhost:5220/api/market");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode} - {errorContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка: {ex.Message}");
        }
    }
}

// Клас для десеріалізації відповіді логіну
class LoginResponse { public required string token { get; set; } } // Поле "token" збігається з JSON

// Клас для десеріалізації помилок
class ErrorResponse { public required string Message { get; set; } }

// Клас для десеріалізації конфігурації (змінено на "token")
class Config { public required string token { get; set; } } // Змінено на "token" замість "Token"