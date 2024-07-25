using Microsoft.EntityFrameworkCore;
using CustomerApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Run();
