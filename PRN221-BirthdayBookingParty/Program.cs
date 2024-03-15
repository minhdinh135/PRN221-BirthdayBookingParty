using DAOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using PRN221_BirthdayBookingParty.Authorization;
using Repositories;
using Repositories.Interfaces;
using Services;
using Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(20));

builder.Services.AddScoped<IRepositoryBase<User>, UserRepository>();
builder.Services.AddScoped<ServiceRepository>();
builder.Services.AddScoped<PackageRepository>();

builder.Services.AddDbContext<BookingPartyContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookingPartyDB"))
);

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization(options =>
    options.AddPolicy("AdminSessionPolicy", policy =>
        {
            policy.Requirements.Add(new SessionRequirement("Admin"));
        }
    )
);
builder.Services.AddAuthorization(options =>
    options.AddPolicy("HostSessionPolicy", policy =>
    {
        policy.Requirements.Add(new SessionRequirement("Host"));
    }
    )
);
builder.Services.AddAuthorization(options =>
    options.AddPolicy("CustomerSessionPolicy", policy =>
    {
        policy.Requirements.Add(new SessionRequirement("Customer"));
    }
    )
);
builder.Services.AddAuthorization(options =>
    options.AddPolicy("LoginSessionPolicy", policy =>
    {
        policy.Requirements.Add(new LoginSessionRequirement(true));
    }
    )
);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/Login";
       }
     );
builder.Services.AddSingleton<IAuthorizationHandler,SessionRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, LoginSessionRequirementHandler>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();


app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
