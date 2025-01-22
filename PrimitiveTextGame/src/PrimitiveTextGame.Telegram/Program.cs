using PrimitiveTextGame.Telegram.Modules.Common.Extensions;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
services.RegisterModules(builder.Configuration);
var host = builder.Build();
var serviceScopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
await services.ApplyMigrationAsync(serviceScopeFactory);
host.Run();
