using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shop.API.Extensions;
using Shop.API.Mapper;
using Shop.API.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddAutoMapper(typeof(AutomapperProfile));
// Add services to the container.
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnectionString")));

builder.Services.AddControllers();

builder.Services.AddAplictionServices();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors(options => 
{
    options.AddPolicy("CorsPolicy", policy => 
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
    });
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<DataContext>();
        await context.Database.MigrateAsync();
        await DataContaxtSeed.SeedAsync(context, loggerFactory);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occured during migration");
        throw;
    }
}

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles();
 
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
