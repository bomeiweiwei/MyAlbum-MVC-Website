using MyAlbum.IoC;
using MyAlbum.Models.Options;
using MyAlbum.Shared.Enums;
using MyAlbum.Shared.Extensions;
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

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "MemberAuth";
        options.DefaultChallengeScheme = "AppAuth";
    })
    .AddPolicyScheme("AppAuth", "AppAuth", options =>
    {
        options.ForwardDefaultSelector = ctx =>
        {
            var path = ctx.Request.Path;
            if (path.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase))
                return "AdminAuth";
            return "MemberAuth";
        };
    })
    .AddCookie("MemberAuth", opt =>
    {
        opt.LoginPath = "/Member/Identity/Login";
        opt.LogoutPath = "/Member/Identity/Logout";
        opt.AccessDeniedPath = "/Member/Identity/Denied";
        opt.Cookie.Name = "memberAuth";
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        opt.SlidingExpiration = true;
        opt.ExpireTimeSpan = TimeSpan.FromHours(8);
    })
    .AddCookie("AdminAuth", opt =>
    {
        opt.LoginPath = "/Admin/Identity/Login";
        opt.LogoutPath = "/Admin/Identity/Logout";
        opt.AccessDeniedPath = "/Admin/Identity/Denied";
        opt.Cookie.Name = "adminAuth";
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        opt.SlidingExpiration = true;
        opt.ExpireTimeSpan = TimeSpan.FromHours(8);
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAuthenticatedUser()
              .AddAuthenticationSchemes("AdminAuth")
              .RequireClaim("AccountType", AccountType.Admin.GetDescription()));

    options.AddPolicy("MemberOnly", policy =>
        policy.RequireAuthenticatedUser()
              .AddAuthenticationSchemes("MemberAuth")
              .RequireClaim("AccountType", AccountType.Member.GetDescription()));
});

builder.Services.AddAntiforgery(o => o.HeaderName = "RequestVerificationToken");

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
).RequireAuthorization("AdminOnly");

app.MapAreaControllerRoute(
    name: "member",
    areaName: "Member",
    pattern: "Member/{controller=Home}/{action=Index}/{id?}"
).RequireAuthorization("MemberOnly");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
