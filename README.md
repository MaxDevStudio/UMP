# UMP Project

## Опис
Цей проєкт є демонстраційним прикладом веб-API та командного рядкового інтерфейсу (CLI), розроблених із використанням .NET. Проєкт демонструє основи авторизації через JWT, взаємодію з базою даних PostgreSQL через Entity Framework Core та просту архітектуру клієнт-сервер.

### Технічні деталі
- **Мови та технології**:
  - C# (.NET 8)
  - Entity Framework Core для роботи з PostgreSQL
  - JWT для авторизації
  - System.CommandLine для CLI
  - Swashbuckle.AspNetCore для Swagger документації

- **Структура проєкту**:
  - `UMP-API`: Веб-API з ендпоінтами для авторизації (`/api/auth/login`) та маркетплейсу (`/api/market`).
  - `UMP-CLI`: Командний рядок для взаємодії з API (команди `login` і `market list`).

- **Налаштування**:
  - База даних: PostgreSQL (локально або через Docker).
  - Конфігурація: `appsettings.json` із ConnectionString та JWT-настройками.
  - Токен зберігається в `~/.ump/config.json` після логіну.

- **Як запустити**:
  1. Переконайся, що .NET 8 SDK встановлено.
  2. Налаштуй PostgreSQL і онови `ConnectionString` у `UMP-API/appsettings.json`.
  3. У папці `UMP-API` виконай:
     ```
      dotnet restore
      dotnet ef database update
      dotnet run
	 ```
  1. 4. У папці `UMP-CLI` виконай:
       ``` 
      dotnet run -- login --username testuser --password testpass
      dotnet run -- market list
     ```
   - **Примітки для розробників**:

Код відкритий для технічних дискусій і вдосконалень.
Фокус на архітектурі та реалізації.
  
   - **План розвитку**:

Додати ендпоінти для CRUD-операцій із маркетплейсом.
Оптимізувати обробку помилок і логування.
Розширити CLI новими командами.

   - **Контрибуція**:
Ласкаво просимо до співпраці! Відкриті технічні питання для обговорення.

