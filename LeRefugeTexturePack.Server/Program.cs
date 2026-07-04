using LeRefugeTexturePack.Server.Interfaces.Services;
using LeRefugeTexturePack.Server.Services;
using LeRefugeTexturePack.Server.Utils;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = "api/swagger";
});


app.UseAuthorization();

app.MapControllers();

app.Run();
