using Microsoft.EntityFrameworkCore;
using PlatformService.Context;
using PlatformService.DataAsyncServices;
using PlatformService.DataSyncServices;
using PlatformService.Repo;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemoryDb"));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("PlatformServiceDb");
    connectionString = connectionString.Replace("{SA_PASSWORD}", Environment.GetEnvironmentVariable("SA_PASSWORD"));

    Console.WriteLine($"Db connection string: {connectionString}");
    builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(connectionString));
}

// Add services to the container.
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// Register to IHttpClientFactory.
builder.Services.AddHttpClient<IDataSyncService, HttpDataSyncService>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

PrepDb.PopulateDummyData(app, app.Environment.IsProduction());


app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<GrpcPlatformService>();

app.Run();
