using Infrastructure;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using GestaoPedido.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));

var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDb") 
    ?? "mongodb://localhost:27017";

var sqlConnectionString = builder.Configuration.GetConnectionString("SqlServer") 
    ?? "Server=localhost;Database=GestaoPrecoDb;Trusted_Connection=True;TrustServerCertificate=True;";

var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase("GestaoPrecoDb");

builder.Services.AddSingleton<IMongoCollection<Order>>(sp =>
    mongoDatabase.GetCollection<Order>("Orders"));

builder.Services.AddSingleton<IMongoCollection<OrderItem>>(sp =>
    mongoDatabase.GetCollection<OrderItem>("OrderItems"));

builder.Services.AddSingleton<IMongoCollection<Product>>(sp =>
    mongoDatabase.GetCollection<Product>("Products"));

builder.Services.AddSingleton<IMongoCollection<Customer>>(sp =>
    mongoDatabase.GetCollection<Customer>("Customers"));

builder.Services.AddSingleton<MongoDbContext>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(Application.ListOrdersQuery).Assembly));




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
