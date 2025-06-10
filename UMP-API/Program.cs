using System.Text; // ��������� ��� ������ � �������
using Microsoft.AspNetCore.Authentication.JwtBearer; // ��������� ��� JWT-�����������
using Microsoft.EntityFrameworkCore; // ��������� ��� ������ � ����� ����� ����� EF Core
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens; // ��������� ��� ��������� � �������� JWT
using UMP_API.Data; // ������ ���� ��� ��������� ���� �����

var builder = WebApplication.CreateBuilder(args);

// �������� ������ ��� ���������� (��� ��������� HTTP-������)
builder.Services.AddControllers();

// ������ Swagger ��� �������� ���������� API � ����� ��������
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ����������� ���������� �� PostgreSQL
builder.Services.AddDbContext<UmpWinDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")); // ������������� PostgreSQL �� ������������
});

// ����������� JWT-�����������
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt"); // �������� ������������ JWT �� appsettings.json
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, // ����������, �� ������� �������� ������
            ValidateAudience = true, // ����������, �� ������ �������� ������
            ValidateLifetime = true, // ���������� ����� 䳿 ������
            ValidateIssuerSigningKey = true, // ���������� ����� ������
            ValidIssuer = jwtSettings["Issuer"], // ������������ �������� ������
            ValidAudience = jwtSettings["Audience"], // ������������ �������� ������
            IssuerSigningKey = new // ��������� ���� ��� ������
            SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!)) // ������ !
         };
    });

var app = builder.Build();

// ��������� Swagger � ����� ��������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ����������� ��������������� �� HTTPS ��� �������
app.UseHttpsRedirection();

// ������ middleware ��� �����������
app.UseAuthentication();
app.UseAuthorization();

// ����������� ������������� ��� ����������
app.MapControllers();

// ����������� ���� ����� � ��������� ��������� �����������
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UmpWinDbContext>(); // �������� �������� ����
    db.Database.EnsureCreated(); // ��������� ����, ���� �� ����
    if (!db.Users.Any()) // ����������, �� � �����������
    {
        db.Users.Add(new User // ������ ��������� �����������
        {
            Username = "testuser", // ��'� �����������
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("testpass"), // ��� ������
            Role = "admin" // ���� �����������
        });
        db.SaveChanges(); // �������� ���� � ���
    }
}

// ��������� �������
app.Run();