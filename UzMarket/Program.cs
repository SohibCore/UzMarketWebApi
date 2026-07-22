using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Commands;
using UzMarket.ServiceLayer.Security;
using UzMarket.ServiceLayer.Services.AuthServices;
using UzMarket.ServiceLayer.Services.CartServices;
using UzMarket.ServiceLayer.Services.OrderServices;
using UzMarket.ServiceLayer.Services.ProductServices;
using UzMarket.ServiceLayer.Services.UserServices;
using UzMarket.Validators.User;

var builder = WebApplication.CreateBuilder(args);

// Swagger ishlash
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL ulanish
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// COOKIE AUTHENTICATION
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/api/Auth/Login";
    options.LogoutPath = "/api/Auth/Logout";
    options.ExpireTimeSpan = TimeSpan.FromHours(7);
    options.SlidingExpiration = true; // har so'rovda muddat 7 soatga uzayadi
    options.Cookie.HttpOnly = true; // JS orqali o'qib bo'lmaydi (XSS himoyasi)
    options.Cookie.SameSite = SameSiteMode.Lax; // Frontend boshqa domenda bo'lsa: None + Secure
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // faqat HTTPS orqali yuboriladi
                                                             // API/SPA uchun: login sahifasiga redirect o'rniga to'g'ri HTTP status qaytarsin
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = 401; // Unauthorized
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = 403; // Forbidden
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

// DATA PROTECTION
builder.Services.AddDataProtection()
.SetApplicationName("UzMarketWebApi")
.PersistKeysToDbContext<AppDbContext>();
builder.Services.AddHttpContextAccessor();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();

//Mediatr
builder.Services.AddMediatR(
    typeof(Program).Assembly,
    typeof(CreateAddressCommand).Assembly
);

//FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// MIDDLEWARE 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();