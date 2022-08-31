using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.API.Extensions;
using Shop.API.Mapper;
using Shop.API.Middleware;
using StackExchange.Redis;
using Role = Core.Entities.Identity.Role;

var  CorsPolicy = "AllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

//builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddAutoMapper(typeof(AutomapperProfile));
// Add services to the container.
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlConnectionString")));
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlIdentityConnectionString")));

builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
    ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnectionString")));


builder.Services.AddControllers();

builder.Services.AddAplictionServices();
builder.Services.AddIdentityServices(configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors(options => 
{
    options.AddPolicy(name: CorsPolicy, policy => 
    {
        //policy.WithOrigins("https://localhost:4200","http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
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

        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<Role>>();
        var identityContext = services.GetRequiredService<IdentityContext>();
        await identityContext.Database.MigrateAsync();
        await IdentityContextSeed.SeedAsync(userManager, roleManager);
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

//app.UseHttpsRedirection();

app.UseStaticFiles();
 
app.UseCors(CorsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
