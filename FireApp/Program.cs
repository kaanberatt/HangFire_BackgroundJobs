using FireApp.BackgroundJobs.Abstract;
using FireApp.BackgroundJobs.Concrete;
using FireApp.DataAccess.Abstract;
using FireApp.DataAccess.Concrete;
using FireApp.DataAccess.Context;
using FireApp.Service.Jobs.Implementations;
using FireApp.Service.Jobs.Interfaces;
using FireApp.Service.Services.Abstract;
using FireApp.Service.Services.Concrete;
using FireApp.Service.Settings;
using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.EntityFrameworkCore;

IConfiguration _configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.Configure<JobSettings>(_configuration.GetSection("JobSettings"));

builder.Services.AddHangfire(config => config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));



builder.Services.AddSingleton<ICronJobStarter, CronJobStarter>();
builder.Services.AddScoped<ISuruHareketleriJob, SuruHareketleriJob>();
builder.Services.AddScoped<IDeleteExcelFilesJob, DeleteExcelFilesJob>();
builder.Services.AddScoped<IHayvanHareketleriJob, HayvanHareketleriJob>();
builder.Services.AddScoped<IReader, Reader>();
builder.Services.AddScoped<ISuruDal, EfSuruDal>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseAuthorization();
app.UseHangfireDashboard("/hangfire", new DashboardOptions()
{
    AppPath = "https://localhost:7297/", //The path for the Back To Site link. Set to null in order to hide the Back To  Site link.
    DashboardTitle = "TELOS Jobs Management",
    //  IsReadOnlyFunc = (DashboardContext context) => true, // If it is true, user can not delete/ enqueue the jobs.
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = _configuration.GetSection("HangfireSettings:UserName").Value,
            Pass = _configuration.GetSection("HangfireSettings:Password").Value
        }
    }    
});
/* start service */
var cronJobStarter = app.Services.GetService<ICronJobStarter>();
cronJobStarter.StartJobs();
app.UseHangfireServer();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


