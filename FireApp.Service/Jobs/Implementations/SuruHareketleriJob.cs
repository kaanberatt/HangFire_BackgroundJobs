using FireApp.BackgroundJobs.Abstract;
using FireApp.BackgroundJobs.Helper;
using FireApp.BackgroundJobs.Models;
using FireApp.Service.Jobs.Interfaces;

namespace FireApp.Service.Jobs.Implementations
{
    public class SuruHareketleriJob : ISuruHareketleriJob
    {
        private readonly IReader _reader;
        private readonly IPendingFilesHelper _pendingFilesHelper;

        public SuruHareketleriJob(IReader reader, IPendingFilesHelper pendingFilesHelper)
        {
            _reader = reader;
            _pendingFilesHelper = pendingFilesHelper;
        }
        List<string> topFolders = new List<string>() { "1. ETAP", "2. ETAP", "3. ETAP", "DSY" };

        public void Run()
        {
            // Suru hareketleri excel'ini okur topFolder'a göre sırasıyla okuma işlemini gerçekleştirir. 
            //Job işlenir
            foreach (var top in topFolders)
            {
                _reader.ReadExcelSuruHareketleri(top, "Sürü Hareketleri");
                Thread.Sleep(3000);
            }
        }

        public List<List<WaitingFilesModel>> GetWaitingFiles()
        {
            List<List<WaitingFilesModel>> values = new List<List<WaitingFilesModel>>();
            foreach(var top in topFolders)
            {
                var result = _pendingFilesHelper.GetPendingFilesForSuruHareketleri(top, "Sürü Hareketleri");
                if (result.Count != 0)
                {
                    values.Add(result);
                }
            }
            return values;
        }
    }
}
