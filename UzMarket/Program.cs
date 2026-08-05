using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UzMarket.RepositoryLayer.DataBase;
using UzMarket.ServiceLayer.MediatorServices.AddressServices.Commands;
using UzMarket.ServiceLayer.Security.AccountServices;
using UzMarket.ServiceLayer.Security.AuthServices;
using UzMarket.ServiceLayer.Services.RegisterServices.Interfaces;
using UzMarket.ServiceLayer.Services.RegisterServices.Services;
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
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

    options.Events = new CookieAuthenticationEvents
    {
        OnValidatePrincipal = async ctx =>
        {
            var userIdClaim = ctx.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !long.TryParse(userIdClaim, out var userId))
            {
                ctx.RejectPrincipal();
                return;
            }

            var dbContext = ctx.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            var exists = await dbContext.Users.AnyAsync(u => u.Id == userId);

            if (!exists)
            {
                ctx.RejectPrincipal();
                await ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        },
        OnRedirectToLogin = ctx =>
        {
            ctx.Response.StatusCode = 401;
            return Task.CompletedTask;
        },
        OnRedirectToAccessDenied = ctx =>
        {
            ctx.Response.StatusCode = 403;
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
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IEmailSender, MailKitEmailSender>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

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