namespace FireApp.Service.Services.Abstract
{
    public interface ICronJobStarter
    {
        void StartJobs();
        void FinishJobs();
    }
}
