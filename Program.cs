using AvalicaoAtak;
using AvalicaoAtak.Interfaces;
using AvalicaoAtak.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IDeviceService, DeviceService>();
builder.Services.AddSingleton<IPingService, PingService>();
builder.Services.AddSingleton<ILogService, LogService>();

var host = builder.Build();
host.Run();
