
using FireApp.BackgroundJobs.Models;

namespace FireApp.BackgroundJobs.Abstract
{
    public interface IReader
    {
        void ReadExcelSuruHareketleri(string etapNo = "1. ETAP", string altKlasor = "Sürü Hareketleri");
        void ReadExcelHayvanVarligi(string etapNo = "1. ETAP", string altKlasor = "Hayvan Varlığı");
        void DeleteSuruHareketleriExcelFiles(string etapNo = "1. ETAP", string altKlasor = "Sürü Hareketleri");
        ResultMessageModel ReadExcelWithMessage(string etapNo , string altKlasor);
    }
}
