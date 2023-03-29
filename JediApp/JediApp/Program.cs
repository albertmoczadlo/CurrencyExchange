// See https://aka.ms/new-console-template for more information
using JediApp.Database.Domain;
using JediApp.Database.Repositories;
using JediApp.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;



//add app admin user
//var adminUser = new User();
//adminUser.Login = "admin";
//adminUser.Password = "admin";
//adminUser.Role = UserRole.Admin;

var currencyPLN = new Currency() //add first currency PLN
{
    Name = "Złoty",
    ShortName = "PLN",
    Country = "Poland",
    BuyAt = 1,
    SellAt = 1
};

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services
            //.AddTransient<ITransientOperation, DefaultOperation>() //example
            //.AddScoped<IScopedOperation, DefaultOperation>()       //example
            //.AddSingleton<ISingletonOperation, DefaultOperation>() //example
            //.AddTransient<OperationLogger>()                       //example
            )
    .Build();

var userService = host.Services.GetRequiredService<UserService>();


var exchangeOfficeBoardService = host.Services.GetRequiredService<ExchangeOfficeBoardService>();
exchangeOfficeBoardService.AddCurrency(currencyPLN);

//var menuService = host.Services.GetRequiredService<MenuService>();
//menuService.WelcomeMenu();



//IUserRepository userRepo = new UserRepository();
//IUserService userService = new UserService(userRepo);
//IExchangeOfficeBoardRepository exchangeOfficeBoardRepo = new ExchangeOfficeBoardRepository();
//IExchangeOfficeBoardService exchangeOfficeBoardSevice = new ExchangeOfficeBoardService(exchangeOfficeBoardRepo);
//IMenuService menuService = new MenuService(userService, exchangeOfficeBoardSevice);

//userService.AddUser(adminUser);
//exchangeOfficeBoardSevice.AddCurrency(currencyPLN);


//test menu
//menuService.WelcomeMenu();


