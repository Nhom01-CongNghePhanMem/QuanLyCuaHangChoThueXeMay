using System.Security;
using Microsoft.EntityFrameworkCore;
using MotorbikeRental.API.Extensions;
using MotorbikeRental.Infrastructure.Data.Contexts;
using MotorbikeRental.Web.Extensions;
using MotorbikeRental.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<MotorbikeRentalDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MotorbikeRentalDB"));
});
builder.Services.AddMemoryCache();
ServiceExtension.Services(builder.Services);
SecurityExtension.RegisterSecurityService(builder.Services, builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5174", "https://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//RESET DATABASE
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<MotorbikeRentalDbContext>();

//    await dbContext.Database.EnsureDeletedAsync();
//    await dbContext.Database.EnsureCreatedAsync();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowFrontend");

#region Custom Middlewares
app.UseExceptionHandling();
app.UseRequestResponseLogging();
#endregion

app.UseAuthorization();

app.MapControllers();

app.Run();
