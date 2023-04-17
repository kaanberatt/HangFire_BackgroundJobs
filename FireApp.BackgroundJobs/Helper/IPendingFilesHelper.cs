using FireApp.BackgroundJobs.Models;

namespace FireApp.BackgroundJobs.Helper
{
    public interface IPendingFilesHelper
    {
        List<WaitingFilesModel> GetPendingFilesForSuruHareketleri(string etapNo, string altKlasor);
    }
}
