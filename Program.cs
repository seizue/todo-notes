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
       policy.AllowAnyOrigin()
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

app.MapGet("/", (HttpContext context) =>
{
    var swaggerUrl = $"{context.Request.Scheme}://{context.Request.Host}/swagger";
    return Results.Text(
        $"<h1>Todo Notes API</h1><p>See the <a href='{swaggerUrl}'>Swagger UI</a>.</p>",
        "text/html"
    );
});

app.Run("http://0.0.0.0:" + (Environment.GetEnvironmentVariable("PORT") ?? "5065"));

