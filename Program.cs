using EmployeeManagementApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQLite database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=employees.db"));

var app = builder.Build();

// Swagger (optional, for testing)
app.UseSwagger();
app.UseSwaggerUI();

// Map controllers
app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Use Elastic Beanstalk port
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");

