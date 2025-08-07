using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS (Allow all for development)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
       policy.WithOrigins("https://seizue.github.io")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Enable serving static files from wwwroot
builder.Services.AddDirectoryBrowser(); 

var app = builder.Build();

// Middleware order matters!
app.UseHttpsRedirection();
app.UseStaticFiles();

// Enable CORS middleware
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
