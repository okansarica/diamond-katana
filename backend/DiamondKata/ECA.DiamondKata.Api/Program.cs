using ECA.DiamondKata.Api.Infrastructure;
using ECA.DiamondKata.BusinessLayer.Abstract;
using ECA.DiamondKata.BusinessLayer.Concrete;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDiamondKatanaService, DiamondKatanaService>();

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/octet-stream"
    });
})
.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
})
.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins",
            builder =>
            {
                builder.WithOrigins(
                        "http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .AllowAnyMethod();
            });
    })
    .AddLogging();

builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.UseMiddleware<ExceptionMiddleware>();
app.UseResponseCompression();
app.UseResponseCaching();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
