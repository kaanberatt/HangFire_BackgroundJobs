using Hangfire;
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
using Microsoft.Extensions.Logging;


IConfiguration _configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
builder.Services.AddScoped<IIntergrationLogDal, EfIntegrationLogDal>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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
//cronJobStarter.StartJobs();


app.UseHangfireServer();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.MapControllers();
app.Run();