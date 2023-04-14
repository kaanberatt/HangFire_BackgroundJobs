using FireApp.BackgroundJobs.Abstract;
using FireApp.Service.Jobs.Interfaces;


namespace FireApp.Service.Jobs.Implementations
{
    public class HayvanHareketleriJob : IHayvanHareketleriJob
    {
        private readonly IReader _reader;
        List<string> topFolders = new List<string>() { "1. ETAP", "2. ETAP", "3. ETAP" };

        public HayvanHareketleriJob(IReader reader)
        {
            _reader = reader;
        }

        public void Run()
        {
            //Buraya Hayvan hareketleri Job'ını tetikleyecek method gelecek.
            foreach (var top in topFolders)
            {
                _reader.ReadExcelHayvanVarligi(top , "Hayvan Varlığı");
                Thread.Sleep(2000);
            }
        }
    }
}
