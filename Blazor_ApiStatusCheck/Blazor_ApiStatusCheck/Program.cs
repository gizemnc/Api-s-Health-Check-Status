using Blazor_ApiStatusCheck.Components;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Blazor_ApiStatusCheck.HealthChecks;
using Blazor_ApiStatusCheck.Data;
using Microsoft.EntityFrameworkCore;
using Blazor_ApiStatusCheck.HealthChecks.Performance_Resources_HC;
using Blazor_ApiStatusCheck.HealthChecks.User_API_HC;


var builder = WebApplication.CreateBuilder(args);



//Servis yapýlandýrmalarý
builder.Services.AddScoped<HttpClient>();
builder.Services.AddHttpClient();


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});
// Add services to the container.
builder.Services.Configure<MemoryCheckOptions>(builder.Configuration.GetSection("MemoryCheckOptions"));
builder.Services.AddDbContext<ApplicationDbContext>(options=> 
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHealthChecks();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();

// Configure health checks
builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database_hc", failureStatus: null, tags: new[] { "database" })
    .AddCheck<ResponseTimeHealthCheck>("response_time_hc", failureStatus: null, tags: new[] { "response" })
    .AddCheck<CpuUsageHealthCheck>("cpu_usage_hc", failureStatus: null, tags: new[] { "cpuUsage" })
    .AddCheck<DiskSpaceHealthCheck>("disk_space_hc", failureStatus: null, tags: new[] { "disk" })
    .AddCheck<NetworkLatencyHealthCheck>("network_latency_hc", failureStatus: null,
                tags: new[] { "network" })
    .AddCheck<AuthenticationHealthCheck>("authentication_hc",
        failureStatus: null,tags: new[] {"authentication"})
    .AddCheck<MemoryHealthCheck>("memory_hc", tags: new[] { "memory" })
    .AddCheck<InventoryHealthCheck>("inventory_api_hc",tags: new[] {"inventory"})
    .AddCheck<StartupHostedServiceHealthCheck>("hosted_service_hc", tags: new[] { "hosted" });


// Örnek geçici yaþam döngüsü tanýmý
builder.Services.AddSingleton<AuthenticationHealthCheck>(sp =>
    new AuthenticationHealthCheck(new HttpClient(), "url", "username", "password")
);
builder.Services.AddSingleton<StartupHostedServiceHealthCheck>();
builder.Services.AddSingleton<NetworkLatencyHealthCheck>(sp =>
        new NetworkLatencyHealthCheck("https://localhost:7288/healthchecks-ui#/healthchecks", 500)); // URL ve maksimum izin verilen gecikme süresi (ms)
builder.Services.AddSingleton<DiskSpaceHealthCheck>(sp =>
        new DiskSpaceHealthCheck("C", 1024));

builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
    options.Delay = TimeSpan.FromSeconds(2);
});


var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Swagger ve Swagger UI'yi ekleyin
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger"; // Swagger UI ana dizinde çalýþsýn
}); 


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.MapHealthChecks("health", new HealthCheckOptions()
{
    //ResultStatusCodes =
    //{
    //    [HealthStatus.Healthy] = StatusCodes.Status200OK,
    //    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
    //    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    //}
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


// Health check endpoint'lerini taglere göre ayýralým.
app.MapHealthChecks("health/memory", new HealthCheckOptions()
{
    Predicate = (check) => check.Tags.Contains("memory"),
     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("health/database", new HealthCheckOptions()
{
    Predicate = (check) => check.Tags.Contains("database"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("health/response", new HealthCheckOptions()
{
    Predicate = (check) => check.Tags.Contains("response"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("health/hosted", new HealthCheckOptions()
{
    Predicate = (check) => check.Tags.Contains("hosted"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("health/cpu", new HealthCheckOptions()
{
    Predicate = (check) => check.Tags.Contains("cpuUsage"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("health/disk", new HealthCheckOptions()
{
    Predicate = (check) => check.Tags.Contains("disk"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("health/network", new HealthCheckOptions()
{
    Predicate = (check) => check.Tags.Contains("network"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("health/authentication", new HealthCheckOptions()
{
    Predicate = (check) => check.Tags.Contains("authentication"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("health/inventory", new HealthCheckOptions()
{
    Predicate = (check) => check.Tags.Contains("inventory"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
//app.MapHealthChecks("health/http", new HealthCheckOptions()
//{
//    Predicate = (check) => check.Tags.Contains("http"),
//    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
//});


app.Run();