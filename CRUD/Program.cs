using Enities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoriesContracts;
using ServiceContracts;
using Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConection"));
    });

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();

builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonService, PersonService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
       name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();


public partial class Program { } //make the auto generated program accessible programmatically