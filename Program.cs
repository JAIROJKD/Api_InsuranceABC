using Api_InsuranceABC.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ?? Habilita Swagger para documentaci�n
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ?? Configura la conexi�n a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

// ?? Configuraci�n en modo desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ?? Redirigir HTTP a HTTPS
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthorization();

app.MapControllers();
app.Run();