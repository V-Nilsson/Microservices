using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

//if (builder.Environment.IsProduction())
//{
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConnection")));
//}
//else
//{
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseInMemoryDatabase("InMemory"));
//}

builder.Services.AddControllers();
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});
var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

PreperationDb.PreperationPopulation(app, app.Environment.IsProduction());

app.Run();
