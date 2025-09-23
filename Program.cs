using DataBaseAPI;
using DataBaseContext;
using DataBaseContext.Repositories;
using Microsoft.EntityFrameworkCore;
using Test.DataModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddTransient<IRepository<Person>, PersonRepository>();

builder.Services.AddDbContext<Context>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        options.EnableSensitiveDataLogging();
    },
    ServiceLifetime.Scoped,
    ServiceLifetime.Scoped);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();