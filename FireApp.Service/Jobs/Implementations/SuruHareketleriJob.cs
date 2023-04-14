using FireApp.BackgroundJobs.Abstract;
using FireApp.Service.Jobs.Interfaces;

namespace FireApp.Service.Jobs.Implementations
{
    public class SuruHareketleriJob : ISuruHareketleriJob
    {
        private readonly IReader _reader;

        public SuruHareketleriJob(IReader reader)
        {
            _reader = reader;
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
    }
}
