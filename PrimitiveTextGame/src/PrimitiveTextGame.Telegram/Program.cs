using PrimitiveTextGame.Telegram;
using PrimitiveTextGame.Telegram.Modules.Common;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.RegisterModules(builder.Configuration);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();