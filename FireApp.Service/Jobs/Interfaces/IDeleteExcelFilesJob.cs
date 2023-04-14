
using Hangfire;

namespace FireApp.Service.Jobs.Interfaces
{
    public interface IDeleteExcelFilesJob
    {

        [AutomaticRetry(Attempts = 5)]
        [JobDisplayName("Delete Excel Files Job")]
        [Queue("default")]
        void DeleteExcel();
    }
}
