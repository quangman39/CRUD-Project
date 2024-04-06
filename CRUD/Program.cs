using CRUD.StartupExtensions;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

//config serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration)//read configuration setting from buil-in configuration

    .ReadFrom.Services(services);
    // read our current app's service  and make them avaiable to serilog

});

//using configure file to add services
builder.Services.ConfigureService(builder.Configuration);


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}



app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
       name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();


public partial class Program { } //make the auto generated program accessible programmatically