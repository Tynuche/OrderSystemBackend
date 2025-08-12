using Microsoft.EntityFrameworkCore;
using OrderSystem.Models;
using OrderSystem.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder. Services.AddDbContext<OrderSysDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderSysDatabase")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.MaxDepth = 64; // optional: increase max depth
    }); ;
builder.Services.AddTransient<IProductsRepository, ProductsRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
