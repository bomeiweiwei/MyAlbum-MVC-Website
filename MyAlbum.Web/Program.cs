using MyAlbum.IoC;
using MyAlbum.Models.Options;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<ConnectionStringsOptions>()
    .Bind(builder.Configuration.GetSection("ConnectionStrings"))
    .Validate(o =>
        !string.IsNullOrWhiteSpace(o.MasterConnection) &&
        !string.IsNullOrWhiteSpace(o.SlaveConnection),
        "ConnectionStrings is not configured properly");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.RegisterService();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
