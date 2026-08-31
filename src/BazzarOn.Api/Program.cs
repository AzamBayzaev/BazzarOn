using BazzarOn.Application;
using BazzarOn.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 2. Встроенный OpenAPI от Microsoft (заменяет SwaggerGen)
builder.Services.AddOpenApi();

// 3. Регистрация сервисов слоёв Application (MediatR) и Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

// 4. Настройка Scalar в режиме разработки
if (app.Environment.IsDevelopment())
{
    // Генерирует эндпоинт /openapi/v1.json
    app.MapOpenApi();

    // Подключает Scalar UI (доступен по адресу /scalar/v1)
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("BazzarOn API")
            .WithTheme(ScalarTheme.Purple);
    });
}

app.UseHttpsRedirection();

// 5. Аутентификация и авторизация
app.UseAuthentication();
app.UseAuthorization();

// 6. Маппинг контроллеров
app.MapControllers();

app.Run();