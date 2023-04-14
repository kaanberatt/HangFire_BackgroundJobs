using FireApp.BackgroundJobs.Abstract;
using FireApp.Service.Jobs.Interfaces;

namespace FireApp.Service.Jobs.Implementations
{
    public class DeleteExcelFilesJob : IDeleteExcelFilesJob
    {
        private readonly IReader _reader;

        public DeleteExcelFilesJob(IReader reader)
        {
            _reader = reader;
        }
        List<string> topFolders = new List<string>() { "1. ETAP", "2. ETAP", "3. ETAP" };
        List<string> subFolders = new List<string>() { "Sürü Hareketleri", "Hayvan Varlığı" };

        // Hayvan varlığı ve Sürü Hareketleri için bu methoddan dosyalar temizlenir.
        public void DeleteExcel()
        {
            //Oluşturulma tarihi 24 saatten eski olan dosyalar silinecek
            foreach (string sub in subFolders)
            {
                foreach (var top in topFolders)
                {
                    _reader.DeleteSuruHareketleriExcelFiles(top, sub);
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
