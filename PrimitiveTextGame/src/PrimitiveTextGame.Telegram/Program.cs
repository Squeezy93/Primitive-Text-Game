using Microsoft.OpenApi.Models;
using PrimitiveTextGame.Telegram.Modules.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.RegisterModules(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
   options.SwaggerDoc("v1", new OpenApiInfo { Title = "Primitive text game api", Version = "v1"});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseStatusCodePages();
//TODO: move getting service inside

await app.ApplyMigrationAsync();
app.MapEndpoints(builder.Configuration);
app.Run();



