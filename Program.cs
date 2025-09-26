using DataBaseAPI;
using DataBaseContext;
using DataBaseContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Test.DataModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddTransient<IRepository<Person>, PersonRepository>();

// builder.Services.AddDbContext<Context>(options =>
//     {
//         options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
//         // options.EnableSensitiveDataLogging();
//     },
//     ServiceLifetime.Scoped,
//     ServiceLifetime.Scoped);

var app = builder.Build();

// Ждем пока БД станет доступна
await WaitForDb(builder.Configuration.GetConnectionString("DefaultConnection"));

async Task WaitForDb(string connString)
{
    const int maxAttempts = 10;
    var attempt = 0;
    
    while (attempt < maxAttempts)
    {
        try
        {
            using var connection = new NpgsqlConnection(connString);
            await connection.OpenAsync();
            Console.WriteLine("Database connected successfully!");
            return;
        }
        catch (Exception ex)
        {
            attempt++;
            Console.WriteLine($"Database connection attempt {attempt} failed: {ex.Message}");
            await Task.Delay(2000); // Ждем 2 секунды
        }
    }
    
    throw new Exception("Could not connect to database after all attempts");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();