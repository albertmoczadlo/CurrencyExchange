using JediApp.Database.Repositories;
using JediApp.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JediApp.Web.Areas.Identity.Data;
using JediApp.Database.Domain;
using Microsoft.Extensions.DependencyInjection;
using JediApp.Services;
using JediApp.Database.Interface;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Configuration;
using JediApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<JediAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("JediAppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'JediAppDbContextConnection' not found.")));

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<JediAppDbContext>();
var connectionString = builder.Configuration.GetConnectionString("JediAppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'JediAppDbContextConnection' not found.");


builder.Services.AddControllers();

builder.Services.AddDbContext<JediAppDbContext>(options =>
            options.UseSqlServer(connectionString));


//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
//    .AddEntityFrameworkStores<JediAppDbContext>();

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
//   .AddEntityFrameworkStores<JediAppDbContext>();

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
            .AddEntityFrameworkStores<JediAppDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();

//builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
//    .AddEntityFrameworkStores<JediAppDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews();

//register repos
builder.Services.AddTransient<IExchangeOfficeBoardRepository, ExchangeOfficeBoardRepositoryDB>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<INbpJsonRepository, NbpJsonRepository>();
builder.Services.AddTransient<ITransactionHistoryRepository, TransactionHistoryRepositoryDB>();
builder.Services.AddTransient<IExchangeOfficeRepository, ExchangeOfficeRepositoryDB>();
builder.Services.AddTransient<IUserWalletRepository, UserWalletRepository>();
builder.Services.AddTransient<IAvailableMoneyOnStockRepository, AvailableMoneyOnStockRepository>();
builder.Services.AddTransient<IUserAlarmsRepository, UserAlarmRepository>();

//register services
builder.Services.AddTransient<IExchangeOfficeBoardService, ExchangeOfficeBoardService>();
builder.Services.AddTransient<UserService>();
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
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
