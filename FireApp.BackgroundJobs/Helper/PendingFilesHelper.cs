using FireApp.BackgroundJobs.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IO;

namespace FireApp.BackgroundJobs.Helper
{
    public class PendingFilesHelper : IPendingFilesHelper
    {
        private readonly IConfiguration _configuration;

        public PendingFilesHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<WaitingFilesModel> GetPendingFilesForSuruHareketleri(string etapNo, string altKlasor)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string etapKlasoruYolu = Path.Combine(_configuration.GetSection("Path:FolderPath").Value, etapNo, altKlasor);
            List<WaitingFilesModel> modelList = new List<WaitingFilesModel>();

            if (Directory.Exists(etapKlasoruYolu))
            {
                foreach (var filePath in Directory.GetFiles(etapKlasoruYolu).Where(f => new FileInfo(f).FullName.Contains("YemMerkeziListe")).OrderBy(f => new FileInfo(f).CreationTime))
                {
                    var fileName = Path.GetFileName(filePath);
                    string fileCreatedDate = File.GetCreationTime(filePath).ToLocalTime().ToString();
                    if (fileName.Contains("YemMerkeziListe"))
                    {
                        FileInfo file = new FileInfo(filePath);
                        modelList.Add(new WaitingFilesModel()
                        {
                            FileName = fileName,
                            FilePath = etapKlasoruYolu,
                            FileCreatedDate = fileCreatedDate,
                        });
                    }
                }
            }
            return modelList;
        }
    }
}
