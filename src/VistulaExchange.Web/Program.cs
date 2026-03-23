using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using VistulaExchange.Database.Domain;
using VistulaExchange.Database.Interface;
using VistulaExchange.Database.Repositories;
using VistulaExchange.Infrastructure.Repositories;
using VistulaExchange.Services.Services.Interfaces;
using VistulaExchange.Services.Services.Service;
using VistulaExchange.Web.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("VistulaExchangeDbContextConnection") ?? throw new InvalidOperationException("Connection string 'VistulaExchangeDbContextConnection' not found.");

builder.Services.AddDbContext<VistulaExchangeDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
    .AddEntityFrameworkStores<VistulaExchangeDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

//register repos
builder.Services.AddTransient<IExchangeOfficeBoardRepository, ExchangeOfficeBoardRepositoryDB>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddHttpClient<INbpJsonRepository, NbpJsonRepository>();
builder.Services.AddTransient<ITransactionHistoryRepository, TransactionHistoryRepositoryDB>();
builder.Services.AddTransient<IExchangeOfficeRepository, ExchangeOfficeRepositoryDB>();
builder.Services.AddTransient<IUserWalletRepository, UserWalletRepository>();
builder.Services.AddTransient<IAvailableMoneyOnStockRepository, AvailableMoneyOnStockRepository>();
builder.Services.AddTransient<IUserAlarmsRepository, UserAlarmRepository>();

//register services
builder.Services.AddTransient<IExchangeOfficeBoardService, ExchangeOfficeBoardService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<INbpJsonService, NbpJsonService>();
builder.Services.AddTransient<ITransactionHistoryService, TransactionHistoryService>();
builder.Services.AddTransient<IExchangeOfficeService, ExchangeOfficeService>();
builder.Services.AddTransient<IUserWalletService, UserWalletService>();
builder.Services.AddTransient<IUserAlarmsService, UserAlarmsService>();
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
