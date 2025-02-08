using Agah.API_s;
using Datalayer.Data;
using Datalayer.Repositories;
using Datalayer.Repositories.IRepositories;
using Hangfire;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// **Apply CORS before Authorization**
app.UseCors("AllowLocalhost");

app.UseAuthorization();

app.UseHangfireDashboard("/Hangfire");

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
