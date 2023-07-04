using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Quartz.Impl;
using Quartz.Spi;
using Viho.Models;
using Viho.web.DataDB;
using Quartz;
using Viho.web.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Viho.Controllers;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddMvc().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbRentalContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Authentication/LoginWithImageTwo";
        options.AccessDeniedPath = "/Authentication/LoginWithImageTwo";
        //options.ExpireTimeSpan = TimeSpan.Zero; // Expire the cookie when the browser is closed
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Role3", policy =>
        policy.RequireClaim("URoleid", "3"));
});

//builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme, options =>
//{
//    options.ExpireTimeSpan = TimeSpan.Zero;
//});                                        //use this to logout everytime browser is closed.

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the desired idle timeout value
});


builder.Services.AddHostedService<ReminderService>();





var app = builder.Build();

app.MapControllers();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseSession(); // Add this line before app.UseRouting()

app.UseRouting();

app.UseAuthentication();

// Add the following middleware to log out the user if accessing the login page
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/Authentication/LoginWithImageTwo" && context.User.Identity.IsAuthenticated)
    {
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        context.Response.Redirect("/Authentication/LoginWithImageTwo");
        return;
    }

    await next();
});

// Add the following middleware to prevent caching on authenticated pages
app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated)
    {
        // Allow caching for non-authenticated pages
        await next();
    }
    else
    {
        // Prevent caching on authenticated pages
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";
        context.Response.Headers["Expires"] = "0";

        await next();
    }
});




app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");




app.Run();
