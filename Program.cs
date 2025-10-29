using EssenceShop.Context;
using EssenceShop.Repositries;
using EssenceShop.Repositries.Interface;
using EssenceShop.Service;
using EssenceShop.Service.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<EssenceDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IClothesServices, ClothesService>();
builder.Services.AddScoped<IClothesRepositries, ClothesRepositries>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddScoped<IClientsRepositries, ClientsRepositries>();






var app = builder.Build();

// Enable Swagger & Swagger UI

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
