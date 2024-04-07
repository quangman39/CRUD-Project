using Enities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoriesContracts;
using ServiceContracts;
using ServiceContracts.IPersonsServices;
using Services;
using Services.PersonsServices;

namespace CRUD.StartupExtensions
{
    public static class ConfigureServicesExtension
    {

        public static IServiceCollection ConfigureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();

            //add services in ioc container
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonsAdderService, PersonsAdderService>();
            services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
            services.AddScoped<IPersonsGetterService, PersonGetterService>();
            services.AddScoped<IPersonsUpdateService, PersonsUpdateService>();
            services.AddScoped<IPersonsSorterService, PersonsSorterService>();

            services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConection"));
                });


            return services;
        }



    }
}
