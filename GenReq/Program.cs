using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Azure.Identity;
using GenReq.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
if (keyVaultEndpoint == null)
{
    Console.Error.WriteLine("VaultUri is not set in the environment.");
    System.Diagnostics.Trace.TraceError("VaultUri is not set in the environment.");
}
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews()
    /*options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
})*/;
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToFolder("/");
    options.Conventions.AllowAnonymousToFolder("/Home");
    options.Conventions.AllowAnonymousToFolder("/Shared");
    options.Conventions.AuthorizeFolder("/GenRequests");
    options.Conventions.AuthorizeFolder("/UserRegistrations");
})
    .AddMicrosoftIdentityUI();

string strconnString = builder.Configuration.GetConnectionString("GenReqContext");
if (strconnString == null)
{
    Console.Error.WriteLine("ConnectionString:GenReqContext is not set in the environment.");
    System.Diagnostics.Trace.TraceError("ConnectionString:GenReqContext is not set in the environment.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(strconnString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
