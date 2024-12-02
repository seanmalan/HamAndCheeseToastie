using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();


// Configure CORS to allow requests from your React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policyBuilder => policyBuilder
            .WithOrigins("http://localhost:3000", "http://10.0.0.2:3000") // React app runs on port 3000 in development
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Swagger for development
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<TokenCacheService>();
builder.Services.AddScoped<ICsvReader, CsvReaderService>();
builder.Services.AddSingleton<EmailService>();
builder.Services.AddSingleton<IRsaService, RsaService>();


// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]; // Recommended to store keys in configuration or environment
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer, // Configured in appsettings.json or environment
        ValidAudience = jwtAudience, // Configured in appsettings.json or environment
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)) // Ensure this key is the same as used in token generation
    };
});

// Database configuration
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// CORS policy for React app
app.UseCors("AllowReactApp");

// Ensure Authentication and Authorization are used in correct order
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
