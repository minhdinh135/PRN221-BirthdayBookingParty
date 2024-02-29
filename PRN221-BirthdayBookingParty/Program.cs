using DAOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using Repositories.Interfaces;
using Repositories;
using Service;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(20));
builder.Services.AddScoped<IRepositoryBase<User>, UserRepository>();

builder.Services.AddDbContext<BookingPartyContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookingPartyDB"))
);


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
