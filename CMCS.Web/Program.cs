using CMCS.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Optional but useful for debugging in the console
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// DI registrations (we'll add these types in steps 2–4)
builder.Services.AddSingleton<IClaimRepository, JsonClaimRepository>();
builder.Services.AddSingleton<IFileCrypto, AesFileCrypto>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
