using System.Globalization;
using System.Net.Http.Headers;
using Epsilon.Abstractions;
using Epsilon.Abstractions.Components;
using Epsilon.Abstractions.Services;
using Epsilon.Components;
using Epsilon.Host.WebApi;
using Epsilon.Host.WebApi.Data;
using Epsilon.Host.WebApi.Options;
using Epsilon.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Tpcly.Canvas.Abstractions.GraphQl;
using Tpcly.Canvas.Abstractions.Rest;
using Tpcly.Canvas.GraphQl;
using Tpcly.Canvas.Rest;
using Tpcly.Lti;
using Tpcly.Persistence.Abstractions;
using Tpcly.Persistence.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add CORS rules
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => policy.WithOrigins(config["Lti:TargetUri"])
                                             .AllowAnyHeader()
                                             .AllowAnyMethod()
                                             .AllowCredentials()));

// Add controllers
builder.Services.AddControllers();

// Add Canvas services
const string canvasHttpClient = "CanvasHttpClient";
var canvasConfiguration = config.GetSection("Canvas");

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

// Add persistence services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = config.GetConnectionString("Default");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

builder.Services.AddScoped<IReadOnlyRepository<LearningDomain>, EntityFrameworkReadOnlyRepository<ApplicationDbContext, LearningDomain>>();
builder.Services.AddScoped<IReadOnlyRepository<LearningDomainOutcome>, EntityFrameworkReadOnlyRepository<ApplicationDbContext, LearningDomainOutcome>>();

// Add domain services
builder.Services.AddScoped<IPageComponentManager, PageComponentManager>();
builder.Services.AddScoped<ICompetenceDocumentService, CompetenceDocumentService>();
builder.Services.AddScoped<IFilterService, FilterService>();
builder.Services.AddScoped<ILearningDomainService, LearningDomainService>();
builder.Services.AddScoped<ILearningOutcomeCanvasResultService, LearningOutcomeCanvasResultService>();

// Add authentication
builder.Services.AddSingleton<LtiSecurityTokenValidator>();
builder.Services.AddLti()
       .AddPlatforms(config.GetSection("Lti").GetSection("Platforms").Get<List<ToolPlatform>>());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer();

builder.Services.ConfigureOptions<LtiJwtBearerOptions>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CanvasUserSession>(static services =>
{
    var context = services.GetRequiredService<IHttpContextAccessor>().HttpContext;
    var ltiMessage = context.GetLtiMessageAsync().Result;

    return new CanvasUserSession(
        int.Parse(ltiMessage.Custom["course_id"].ToString(), CultureInfo.InvariantCulture),
        int.Parse(ltiMessage.Custom["user_id"].ToString(), CultureInfo.InvariantCulture)
    );
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(static options =>
{
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseSwagger();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();