using Enities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoriesContracts;
using ServiceContracts;
using Services;

namespace CRUD.StartupExtensions
{
    public static class ConfigureServicesExtension
    {

        public static IServiceCollection ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonService, PersonService>();

            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConection"));
                });


            return services;
        }



    }
}
