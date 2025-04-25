using Agah.Extensions;
using Agah.Filters;
using Agah.Services;
using Datalayer.Data;
using Datalayer.Models;
using Datalayer.Repositories;
using Datalayer.Repositories.IRepositories;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Register EmailService
builder.Services.ConfigureEmailService(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ReserveService>();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173")  // Allow frontend URL
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Allow credentials if needed
    });
});

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

// Add Hangfire services
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

        if (!dbContext.Product.Any())
        {
            dbContext.Product.AddRange(
                new Product { PersianName = "طلای 18عیار", EnglishName = "price18", IconName = "fa-solid fa-ring", CreatedAt = DateTime.Now }
            );

            dbContext.Alarm.AddRange(
                new Alarm { PersianName = "نوتیف درون برنامه", AlarmPrice = 10000, CreatedAt = DateTime.Now, EnglishName = "Alert", IsActive = true, ShortDescription = "اطلاع رسانی با نوتیفیکیشن" },
                new Alarm { PersianName = "ایمیل", AlarmPrice = 10000, CreatedAt = DateTime.Now, EnglishName = "Email", IsActive = true, ShortDescription = "اطلاع رسانی با ایمیل" },
                new Alarm { PersianName = "پیامک", AlarmPrice = 10000, CreatedAt = DateTime.Now, EnglishName = "SMS", IsActive = false, ShortDescription = "اطلاع رسانی با پیامک" },
                new Alarm { PersianName = "تلفن", AlarmPrice = 10000, CreatedAt = DateTime.Now, EnglishName = "Phone", IsActive = false, ShortDescription = "اطلاع رسانی با تماس تلفنی" }
                );

            dbContext.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **Apply CORS before Authorization**
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/Hangfire", new DashboardOptions
{
    Authorization = new[] { new BasicAuthAuthorizationFilter("admin", "2ea34b097a6a3bb800f0a13108b92d93") }
});


app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var productService = services.GetRequiredService<ProductService>();
    var reserveService = services.GetRequiredService<ReserveService>();
    var apiJobService = new ApiJobService(productService, reserveService);

    RecurringJob.AddOrUpdate(
        "update-product-log",
        () => apiJobService.CallUpdateProductLogApi(),
        Cron.Minutely);

    RecurringJob.AddOrUpdate(
        "check-price-in-reserve",
        () => apiJobService.CallCheckPriceInReservedsApi(),
        Cron.Minutely);
}

app.Run();
