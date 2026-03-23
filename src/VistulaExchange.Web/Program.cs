using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Infrastructure;
using VistulaExchange.Infrastructure.Persistence;
using VistulaExchange.Infrastructure.Repositories;
using VistulaExchange.Services;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Services.Services.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddHttpClient<INbpJsonRepository, NbpJsonRepository>();

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
    .AddEntityFrameworkStores<VistulaExchangeDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IEmailSender, EmailSender>(i =>
                new EmailSender(
                    builder.Configuration.GetSection("EmailSender:Host").Value,
                    Convert.ToInt32(builder.Configuration.GetSection("EmailSender:Port").Value),
                    Convert.ToBoolean(builder.Configuration.GetSection("EmailSender:EnableSSL").Value),
                    builder.Configuration.GetSection("EmailSender:UserName").Value,
                    builder.Configuration.GetSection("EmailSender:Password").Value
                )
            );
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
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
