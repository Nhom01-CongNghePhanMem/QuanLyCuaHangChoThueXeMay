using Microsoft.EntityFrameworkCore;
using MotorbikeRental.Infrastructure.Data.Contexts;
using MotorbikeRental.Web.Extensions;
using MotorbikeRental.Web.Middlewares;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MotorbikeRentalDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MotorbikeRentalDB")).UseLazyLoadingProxies();
});
ServiceExtension.RegisterServices(builder.Services);
builder.Services.AddMemoryCache();
var app = builder.Build();
//RESET DATABASE
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<MotorbikeRentalDbContext>();

//    await dbContext.Database.EnsureDeletedAsync();
//    await dbContext.Database.EnsureCreatedAsync();
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePages(async context =>
{
    var statusCode = context.HttpContext.Response.StatusCode;

    if (statusCode == 404)
    {
        context.HttpContext.Response.ContentType = "text/html";

        // Đọc file NotFound.cshtml và render
        var viewPath = Path.Combine(app.Environment.ContentRootPath, "Views", "Shared", "NotFound.cshtml");
        if (File.Exists(viewPath))
        {
            var htmlContent = await File.ReadAllTextAsync(viewPath);
            await context.HttpContext.Response.WriteAsync(htmlContent);
        }
        else
        {
            await context.HttpContext.Response.WriteAsync(@"
                <!DOCTYPE html>
                <html>
                <head><title>404 - Không tìm thấy</title></head>
                <body><h1>404 - Trang không tồn tại</h1></body>
                </html>");
        }
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
#region Custom Middlewares
app.UseRequestResponseLogging();
#endregion

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Motorbike}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
