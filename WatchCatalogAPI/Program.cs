using WatchCatalogAPI.Class;
using WatchCatalogAPI.Class.Core;
using WatchCatalogAPI.Repository.Interface;
using WatchCatalogAPI.Repository.UnitofWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IAdapter, Adapter>();
builder.Services.AddSingleton<Connection>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
