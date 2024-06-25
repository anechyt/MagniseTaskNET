using MagniseTaskNET.Application.Infrastructure.Extensions;
using MagniseTaskNET.WebApi.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
