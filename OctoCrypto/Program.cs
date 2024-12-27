using OctoCrypto.Configuration;
using OctoCrypto.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services
    .Configure<MexcApiOptions>(builder.Configuration.GetSection(MexcApiOptions.MexcApi))
    .Configure<BingXApiOptions>(builder.Configuration.GetSection(BingXApiOptions.BingXApi))
    .Configure<GateApiOptions>(builder.Configuration.GetSection(GateApiOptions.GateApi))
    .AddAppServices()
    .AddCustomizedQuartz();

var app = builder.Build();

await app.StartScheduler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace OctoCrypto
{
    // Required for integration tests
    public partial class Program
    {
    }
}