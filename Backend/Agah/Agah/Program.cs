using Agah.Filters;
using Agah.Services;
using Datalayer.Data;
using Datalayer.Repositories;
using Datalayer.Repositories.IRepositories;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Allow frontend URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Allow credentials if needed
    });
});

// Add Hangfire services
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// 🔹 Add Swagger services
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    // 🔹 Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your JWT token}'"
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

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **Apply CORS before Authorization**
app.UseCors("AllowLocalhost");

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/Hangfire", new DashboardOptions
{
    Authorization = new[] { new BasicAuthAuthorizationFilter("admin", "2ea34b097a6a3bb800f0a13108b92d93") }
});


app.MapControllers();

RecurringJob.AddOrUpdate(
    "update-product-log",
    () => ApiJobService.CallUpdateProductLogApi(),
    Cron.Minutely);

RecurringJob.AddOrUpdate(
    "check-price-in-reserve",
    () => ApiJobService.CallCheckPriceInReservedsApi(),
    Cron.Minutely);
app.Run();
