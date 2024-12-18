using PrimitiveTextGame.Telegram.Modules.Common;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.RegisterModules(builder.Configuration);
var host = builder.Build();
host.Run();
