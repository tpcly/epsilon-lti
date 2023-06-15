using System.Net.Http.Headers;
using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;
using Epsilon.Canvas.Abstractions;
using Epsilon.Canvas.Abstractions.GraphQl;
using Epsilon.Canvas.Abstractions.Rest;
using Epsilon.Canvas.GraphQl;
using Epsilon.Canvas.Rest;
using Epsilon.Components;
using Epsilon.Host.WebApi.Options;
using Epsilon.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add CORS rules
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.WithOrigins(builder.Configuration["Lti:TargetUri"]).AllowCredentials()));

// Add controllers
builder.Services.AddControllers();

// Add Canvas services
const string canvasHttpClient = "CanvasHttpClient";
var canvasConfiguration = builder.Configuration.GetSection("Canvas");

builder.Services.Configure<CanvasMockOptions>(canvasConfiguration);
builder.Services.AddHttpClient(
    canvasHttpClient,
    static (provider, client) =>
    {
        var settings = provider.GetRequiredService<IOptions<CanvasMockOptions>>().Value;

        client.BaseAddress = settings.ApiUrl;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.AccessToken);
    });

builder.Services.AddScoped<ICanvasRestApi, CanvasRestApi>();
builder.Services.AddHttpClient<ICanvasGraphQlApi, CanvasGraphQlApi>(canvasHttpClient);

builder.Services.AddHttpClient<IPageEndpoint, PageEndpoint>(canvasHttpClient);
builder.Services.AddHttpClient<IFileEndpoint, FileEndpoint>(canvasHttpClient);
builder.Services.AddHttpClient<IAccountEndpoint, AccountEndpoint>(canvasHttpClient);

builder.Services.AddScoped<CanvasUserSession>(static services =>
{
    var options = services.GetRequiredService<IOptions<CanvasMockOptions>>().Value;
    return new CanvasUserSession(options.CourseId, options.StudentId, options.AccessToken);
});

builder.Services.AddScoped<PageComponentFetcher>();
builder.Services.AddScoped<ICompetenceDocumentService, CompetenceDocumentService>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseSwagger();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();