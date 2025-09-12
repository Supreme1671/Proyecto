var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// CORS: permitir solo el front dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Ajustá si tu React usa https
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();    // opcional, pero útil si quieres https
app.UseCors("AllowReact");    // <<-- IMPORTANTE: antes de MapControllers

app.MapControllers();

app.Run();