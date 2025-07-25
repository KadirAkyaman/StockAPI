using NewAPI.Repositories;
using StockApp.Core.Interfaces;
using StockApp.Core.Options;
using StockApp.Core.Repositories;
using StockApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddScoped(typeof(IProductRepository), typeof(ProductRepository));
builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();

