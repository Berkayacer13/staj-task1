using Microsoft.EntityFrameworkCore;
using CompanyService.Data;
using CompanyService.Services;
using CompanyService.Middleware;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Company Service API",
        Version = "v1",
        Description = "API for managing companies and related data"
    });
});

// Configure API Key authentication
builder.Services.Configure<ApiKeyOptions>(options =>
{
    options.ApiKey = builder.Configuration["ApiKey"] ?? "default-api-key";
});

// Add HttpClient factory for external API calls
builder.Services.AddHttpClient();

// Register services
builder.Services.AddScoped<IAraciKurumService, AraciKurumService>();
builder.Services.AddScoped<IFirmaLogoLinkleriService, FirmaLogoLinkleriService>();
builder.Services.AddScoped<IExternalCompanyService, ExternalCompanyService>();
builder.Services.AddStackExchangeRedisCache(options =>
{
options.Configuration = builder.Configuration.GetConnectionString("Redis");
options.InstanceName = "CompanyService";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Company Service API V1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at root URL
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add middleware
app.UseMiddleware<MetricsMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();

// Enable routing and map controllers
app.UseRouting();
app.MapControllers();

// Add Prometheus metrics endpoint
app.UseMetricServer();
app.UseHttpMetrics();

app.Run();
