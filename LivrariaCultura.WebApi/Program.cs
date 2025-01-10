using LivrariaCultura.Domain.Extensions;
using LivrariaCultura.Domain.Interfaces;
using LivrariaCultura.Domain.Persistence;
using LivrariaCultura.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDatabase>(provider => new MySqlDbContext(builder.Configuration.GetConnectionString("LivrariaCulturaDbConnectionString")));
builder.Services.AddScopedFromNamespace("LivrariaCultura.Domain", "Repositories");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/categorias", async (CategoriaRepository categoriaRepository) =>
{
    var categorias = await categoriaRepository.GetListAsync();
    return Results.Ok(categorias);
});

app.Run();