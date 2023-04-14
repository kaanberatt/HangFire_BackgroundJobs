using FireApp.Service.Jobs.Interfaces;
using FireApp.Service.Services.Abstract;
using FireApp.Service.Settings;
using Hangfire;
using Microsoft.Extensions.Options;

namespace FireApp.Service.Services.Concrete
{
    public class CronJobStarter : ICronJobStarter
    {
        private readonly RecurringJobSettings _recurringJobSettings;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public CronJobStarter(IOptions<JobSettings> jobSettingsOptions, IBackgroundJobClient backgroundJobClient)
        {
            _recurringJobSettings = jobSettingsOptions.Value.RecurringJobSettings;
            _backgroundJobClient = backgroundJobClient;
        }
        private RepetitiveJob SuruHareketleriJob => _recurringJobSettings.SuruHareketleriJob;
        private RepetitiveJob DeleteExcelFilesJob => _recurringJobSettings.DeleteExcelFilesJob;
        private RepetitiveJob HayvanHareketleriJob => _recurringJobSettings.HayvanHareketleriJob;

        public void FinishJobs()
        {
            RecurringJob.RemoveIfExists(SuruHareketleriJob.JobId);
            _backgroundJobClient.Delete(SuruHareketleriJob.JobId);

            RecurringJob.RemoveIfExists(HayvanHareketleriJob.JobId);
            _backgroundJobClient.Delete(HayvanHareketleriJob.JobId);

            RecurringJob.RemoveIfExists(DeleteExcelFilesJob.JobId);
            _backgroundJobClient.Delete(DeleteExcelFilesJob.JobId);
        }

        public void StartJobs()
        {
            // Recurring Jobs
            RecurringJob.AddOrUpdate<ISuruHareketleriJob>(
                        SuruHareketleriJob.JobId, 
                        x => x.Run(), 
                        SuruHareketleriJob.IntervalPattern, // interval değeri
                        TimeZoneInfo.Local, 
                        SuruHareketleriJob.Queue); 
            
            RecurringJob.AddOrUpdate<IDeleteExcelFilesJob>(
                        DeleteExcelFilesJob.JobId, 
                        x => x.DeleteExcel(),
                        DeleteExcelFilesJob.IntervalPattern, 
                        TimeZoneInfo.Local, 
                        DeleteExcelFilesJob.Queue);

            RecurringJob.AddOrUpdate<IHayvanHareketleriJob>(
                        HayvanHareketleriJob.JobId,
                        x => x.Run(),
                        HayvanHareketleriJob.IntervalPattern,
                        TimeZoneInfo.Local,
                        HayvanHareketleriJob.Queue);
        }
    }
}
