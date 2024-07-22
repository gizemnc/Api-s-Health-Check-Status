

var builder = WebApplication.CreateBuilder(args);


    

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();


builder.Services.AddHealthChecksUI().AddInMemoryStorage();


//builder.Services.AddHealthChecksUI(setupSettings: setup =>
//{
//    setup.AddHealthCheckEndpoint("default", "/health");
//}).AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();


#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();

    endpoints.MapHealthChecksUI(setup =>
    { 
        setup.AddCustomStylesheet("wwwroot/css/dotnet.css");
    });
    //endpoints.MapHealthChecksUI(setupOptions =>  Arayüz yapýlandýrmasý denedim olmadý
    //{
    //    setupOptions.UIPath = "/hc-ui";
    //    setupOptions.ResourcesPath = "/ui/resources";
    //    setupOptions.ApiPath = "/ui/api";
    //});
});
#pragma warning restore ASP0014



app.Run();


