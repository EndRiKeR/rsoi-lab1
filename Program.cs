using DataBaseAPI;
using DataBaseContext;
using DataBaseContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Test.DataModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddTransient<IRepository<Person>, PersonRepository>();

var dbHost = builder.Configuration["DB_HOST"] ?? "postgres";
var dbPort = builder.Configuration["DB_PORT"] ?? "5432";
var dbName = builder.Configuration["DB_NAME"];
var dbUser = builder.Configuration["DB_USER"];
var dbPassword = builder.Configuration["DB_PASSWORD"];

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};";

builder.Services.AddDbContext<Context>(options => { options.UseNpgsql(connectionString); });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();