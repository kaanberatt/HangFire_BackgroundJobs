using FireApp.API.Dto;
using FireApp.DataAccess.Abstract;
using FireApp.Entities.Concrete;
using FireApp.Service.Services.Abstract;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;

namespace FireApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly IIntergrationLogDal _intergrationLogDal;
        private readonly ICronJobStarter _cronJobStarter;
        private readonly IStorageConnection _connection;

        public HangfireController(ICronJobStarter cronJobStarter, IIntergrationLogDal ıntergrationLogDal)
        {
            _connection = JobStorage.Current.GetConnection();
            _cronJobStarter = cronJobStarter;
            _intergrationLogDal = ıntergrationLogDal;
        }
        [HttpGet("RunIntegration")]
        public IActionResult RunIntegration(bool isActive)
        {
            try
            {
                // Veritabanından son yapılmış entegrasyon durumunu gösteren değeri verir
                var integrationStatus = _intergrationLogDal.GetListAll(null,x => x.OrderBy(x => x.Id))?.Result?.LastOrDefault()?.isActive;
                if (integrationStatus == null)
                {
                    integrationStatus = false;
                }
                if (integrationStatus == false && isActive == true) // Entegrasyon kapalı , true değeri geldi, entegrasyonu aç
                {
                    _cronJobStarter.StartJobs();
                    var log = new IntegrationLog
                    {
                        Date = DateTime.Now,
                        UserName = "admin", // ileride sisteme giriş yapmış kişinin bilgileriden gelecek.
                        Description = "Entegrasyon başlatıldı",
                        isActive = true
                    };
                    _intergrationLogDal.Add(log);
                    return Ok("Entegrasyon açık");
                }
                else if (integrationStatus == true && isActive == true) // entegrasyon zaten açık
                {
                    return Ok("Entegrasyon zaten açık");
                }
                else if(integrationStatus == true && isActive == false) // entegrasyon durumu açık kapatmak isteniyor , entegrasyonu kapat 
                {
                    _cronJobStarter.FinishJobs();
                    var log = new IntegrationLog
                    {
                        Date = DateTime.Now,
                        UserName = "admin", // ileride sisteme giriş yapmış kişinin bilgileriden gelecek.
                        Description = "Entegrasyon kapatıldı",
                        isActive = false
                    };
                    _intergrationLogDal.Add(log);
                    return Ok("Entegrasyon kapatıldı");
                }
                else
                {
                    return Ok("Entegrasyon durumu zaten kapalı");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException);
            }
        }
        [HttpGet("GetJobStatus")]
        public IActionResult GetJobStatus(string jobId)
        {
            var job = _connection.GetRecurringJobs().Find(x => x.Id == jobId);
            if (job != null)
            {
                if (job.LastExecution == null)
                {
                    var jobStatus = new JobStatusDto
                    {
                        JobId = jobId,
                        LastExecution = string.Empty,
                        NextExecution = job.NextExecution?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        Status = string.Empty,

                    };
                    return Ok(jobStatus);

                }
                else
                {
                    var status = _connection?.GetJobData(job.LastJobId)?.State;
                    var lastExecution = job.LastExecution?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                    var nextExecution = job.NextExecution?.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                    var jobStatus = new JobStatusDto
                    {
                        JobId = jobId,
                        LastExecution = lastExecution.ToString(),
                        NextExecution = nextExecution.ToString(),
                        Status = status.ToString(),
                    };
                    return Ok(jobStatus);
                }
            }
            return NotFound("Job Bulunamadı");
        }
    }
}
