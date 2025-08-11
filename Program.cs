using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();

// Serve `/docs` folder
var docsPath = Path.Combine(Directory.GetCurrentDirectory(), "docs");
if (Directory.Exists(docsPath))
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(docsPath),
        RequestPath = "/docs"
    });
}

// Enable CORS
app.UseCors("AllowAll");

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Authorization
app.UseAuthorization();

app.MapControllers();

// Custom landing page at "/"
app.MapGet("/", (HttpContext context) =>
{
    var swaggerUrl = $"{context.Request.Scheme}://{context.Request.Host}/swagger";

    return Results.Text(
        $"""
        <h1>Todo Notes API</h1>
        <p>See the <a href='{swaggerUrl}'>Swagger UI</a>.</p>
    
        """,
        "text/html"
    );
});

app.Run("http://0.0.0.0:" + (Environment.GetEnvironmentVariable("PORT") ?? "5065"));
