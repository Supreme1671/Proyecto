using Microsoft.EntityFrameworkCore;
using NetflixLibrosAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Conexión con MySQL (lee el connection string de appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NetflixLibrosContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// 🔹 Agregar controladores
builder.Services.AddControllers();

// 🔹 Configurar CORS para permitir llamadas desde React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Frontend en React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 🔹 Configuración de entorno
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

// 🔹 Habilitar CORS (antes de MapControllers)
app.UseCors("AllowReact");

// 🔹 Mapear controladores (API endpoints)
app.MapControllers();

app.Run();
