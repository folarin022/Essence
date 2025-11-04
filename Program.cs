using EssenceShop.Context;
using EssenceShop.Repositries;
using EssenceShop.Repositries.Interface;
using EssenceShop.Service;
using EssenceShop.Service.Interface;
using EssenceShop.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;



Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog();


builder.Services.AddDbContext<EssenceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IClothesServices, ClothesService>();
builder.Services.AddScoped<IClothesRepositries, ClothesRepositries>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddScoped<IClientsRepositries, ClientsRepositries>();
builder.Services.AddScoped<JwtService>();

var key = Encoding.UTF8.GetBytes("THIS_IS_YOUR_SECRET_KEY_32_CHARS_MINIMUM");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuer = false,
ValidateAudience = false,
ValidateLifetime = true,
ValidateIssuerSigningKey = true,
IssuerSigningKey = new SymmetricSecurityKey(key)
};
});

builder.Services.AddAuthorization();


builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "EssenceShop API v1",
        Description = "EssenceShop API - Version 1"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "EssenceShop API v2",
        Description = "EssenceShop API - Version 2"
    });


    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by your JWT token."
    });




    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});








var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EssenceShop API v1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "EssenceShop API v2");
    });


    app.MapControllers();

    app.Run();
}

