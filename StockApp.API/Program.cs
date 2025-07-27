using NewAPI.Repositories;
using StockApp.Core.Interfaces;
using StockApp.Core.Options;
using StockApp.Infrastructure.Repositories;
using StockApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();

