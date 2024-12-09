using LivrariaCultura.ConsoleApp;
using LivrariaCultura.ConsoleApp.Extensions;
using LivrariaCultura.ConsoleApp.Screens;
using LivrariaCultura.Domain.Interfaces;
using LivrariaCultura.Domain.Models;
using LivrariaCultura.Domain.Persistence;
using LivrariaCultura.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();

var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<App>();
serviceCollection.AddSingleton<CategoriasScreenManager>();
serviceCollection.AddSingleton<ProdutosScreenManager>();
serviceCollection.AddScoped<IDatabase>(provider => new MySqlDbContext(configuration.GetConnectionString("LivrariaCulturaDbConnectionString")));
serviceCollection.AddScopedFromNamespace("LivrariaCultura.Domain", "Repositories");

var serviceProvider = serviceCollection.BuildServiceProvider();
var app = serviceProvider.GetRequiredService<App>();

await app.RunAsync();
