using FireApp.BackgroundJobs.Models;
using Hangfire;

namespace FireApp.Service.Jobs.Interfaces
{
    public interface ISuruHareketleriJob
    {
        [AutomaticRetry(Attempts = 5)]
        [JobDisplayName("Suru Hareketleri Job")]
        [Queue("default")]
        void Run();

        List<List<WaitingFilesModel>> GetWaitingFiles();

    }
}
