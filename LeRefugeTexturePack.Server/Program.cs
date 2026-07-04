using LeRefugeTexturePack.Server.Interfaces.Services;
using LeRefugeTexturePack.Server.Services;
using LeRefugeTexturePack.Server.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IDownloadService, DownloadService>();
builder.Services.AddScoped<ITexturesService, TexturesService>();
builder.Services.AddScoped<IZipService, ZipService>();
builder.Services.AddScoped<NoCacheFilter>();


var app = builder.Build();


if (app.Environment.IsProduction())
{
    app.UseDefaultFiles();
    app.MapStaticAssets();
    app.MapFallbackToFile("/index.html");
}

app.MapOpenApi();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
    options.RoutePrefix = "api/swagger";
});


app.UseAuthorization();

app.MapControllers();

app.Run();
