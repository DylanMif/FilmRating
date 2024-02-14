using FilmRating.Models.DataManager;
using FilmRating.Models.EntityFramework;
using FilmRating.Repository;
using Microsoft.EntityFrameworkCore;
using TP3Console.Models.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FilmDBContext>(options =>
  options.UseNpgsql(builder.Configuration.GetConnectionString("FilmDB")));
builder.Services.AddScoped<IDataRepository<Utilisateur>, UtilisateurManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
