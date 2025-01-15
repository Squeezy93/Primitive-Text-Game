using PrimitiveTextGame.Telegram.Modules.Common;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;
services.RegisterModules(builder.Configuration);
var serviceScopeFactory = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
await services.ApplyMigrationAsync(serviceScopeFactory);
var host = builder.Build();
host.Run();
