using Form.Data;
using Microsoft.EntityFrameworkCore;
using WbApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure MySQL connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
builder.Services.AddDbContext<MainDbContext>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
//session management
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient(); // This will register HttpClient for dependency injection

// Add CORS policy (if needed for frontend-backend communication)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        // Explicitly specify the allowed frontend origin (replace with your actual frontend URL)
        builder.WithOrigins("http://localhost:5175", "https://localhost:7242" , "http://localhost:5006")  // Allow only this origin
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();  // Allow credentials (cookies)
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS if needed for frontend-backend interaction
app.UseCors("AllowSpecificOrigin");

//app.UseHttpsRedirection();
app.UseSession();
app.UseAuthorization();

app.MapControllers();

app.Run();
