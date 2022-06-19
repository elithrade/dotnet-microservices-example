using Microsoft.EntityFrameworkCore;
using PlatformService.Context;
using PlatformService.DataSyncServices;
using PlatformService.Repo;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

// Register to IHttpClientFactory.
builder.Services.AddHttpClient<IDataSyncService, HttpDataSyncService>();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

PrepDb.PopulateDummyData(app);

app.Run();
