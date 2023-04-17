using ExcelDataReader;
using FireApp.BackgroundJobs.Abstract;
using FireApp.BackgroundJobs.Models;
using FireApp.DataAccess.Abstract;
using FireApp.Entities.Concrete;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text;

namespace FireApp.BackgroundJobs.Concrete
{
    public class Reader : IReader
    {
        private readonly ISuruDal suruDal;
        private readonly IConfiguration _configuration;
        public Reader(ISuruDal suruDal, IConfiguration configuration)
        {
            this.suruDal = suruDal;
            _configuration = configuration;
        }
        public void ReadExcelSuruHareketleri(string etapNo = "1. ETAP", string altKlasor = "Sürü Hareketleri")
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string etapKlasoruYolu = Path.Combine(_configuration.GetSection("Path:FolderPath").Value, etapNo, altKlasor);

            if (Directory.Exists(etapKlasoruYolu))
            {
                foreach (var filePath in Directory.GetFiles(etapKlasoruYolu).Where(f => new FileInfo(f).FullName.Contains("YemMerkeziListe")).OrderBy(f => new FileInfo(f).CreationTime))
                {
                    List<SuruHareketleri> modelList = new List<SuruHareketleri>();
                    var fileName = Path.GetFileName(filePath);

                    if (fileName.Contains("YemMerkeziListe"))
                    {
                        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                if (reader.RowCount < 1)
                                {
                                    // Excel var ama içerisi boş demektir. Bir dosyada hata olduğu zaman ondan sonra gelen dosyalar işlememelidir.
                                    throw new Exception("Excel boş veya doğru formatta değil");
                                }
                                else
                                {
                                    // Verileri işleme
                                    DataSet result = reader.AsDataSet();
                                    DataTable table = result.Tables[0];

                                    var startIndex = 2; //kaçıncı indexten başlıyorsa o index değeri verilir. (Başlıktan sonraki index)
                                    for (int i = startIndex; i < table.Rows.Count; i++)
                                    {
                                        string kupe = table.Rows[i][0].ToString(); // kupeNo
                                        string padok = table.Rows[i][1].ToString(); // Padok

                                        if (kupe != null && padok != null)
                                        {
                                            // Excelden alınan datalar model'e dolduruluyor.
                                            modelList.Add(new SuruHareketleri
                                            {
                                                CreatedDate = DateTime.Now,
                                                kupeNo = kupe,
                                                Padok = padok,

                                            });
                                        }

                                    }
                                    foreach (var model in modelList)
                                    {
                                        suruDal.Add(model); // Veritabanına ekleme işlemi 
                                    }
                                }
                            }
                        }

                        string newFileName = fileName.Replace("YemMerkeziListe ", "X"); // Dosya işlendikten sonra adını değiştiriyoruz. bir daha işlememek için
                        string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);
                        File.Copy(filePath, newFilePath);
                        File.Delete(filePath); // Eski dosya siliniyor
                    }
                }
            }
        }
        public void ReadExcelHayvanVarligi(string etapNo = "1. ETAP", string altKlasor = "Hayvan Varlığı")
        {
            // Hayvan varlığı buraya yazılacak.
        }
        public void DeleteSuruHareketleriExcelFiles(string etapNo = "1. ETAP", string altKlasor = "Sürü Hareketleri")
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string etapKlasoruYolu = Path.Combine(_configuration.GetSection("Path:FolderPath").Value, etapNo, altKlasor);
            if (Directory.Exists(etapKlasoruYolu))
            {
                List<string> excelFiles = Directory.GetFiles(etapKlasoruYolu).Where(x => new FileInfo(x).CreationTime < DateTime.Now.AddDays(-5)).ToList();
                foreach (var filePath in excelFiles)
                {
                    // Dosyanın oluşturulma tarihini kontrol eder
                    // Dosya oluşturulma tarihi 5 günden önce ise dosyayı silecek
                    // Dosyayı silin
                    File.Delete(filePath);
                }
            }
        }

        public ResultMessageModel ReadExcelWithMessage(string etapNo, string altKlasor)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string etapKlasoruYolu = Path.Combine(_configuration.GetSection("Path:FolderPath").Value, etapNo, altKlasor);

            if (Directory.Exists(etapKlasoruYolu))
            {
                foreach (var filePath in Directory.GetFiles(etapKlasoruYolu).Where(f => new FileInfo(f).FullName.Contains("YemMerkeziListe")).OrderBy(f => new FileInfo(f).CreationTime))
                {
                    List<SuruHareketleri> modelList = new List<SuruHareketleri>();
                    var fileName = Path.GetFileName(filePath);
                    try
                    {
                        if (fileName.Contains("YemMerkeziListe"))
                        {
                            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                            {
                                using (var reader = ExcelReaderFactory.CreateReader(stream))
                                {
                                    if (reader.RowCount < 1)
                                    {
                                        // Excel var ama içerisi boş demektir. Bir dosyada hata olduğu zaman ondan sonra gelen dosyalar işlememelidir.
                                        return new ResultMessageModel
                                        {
                                            FileName = fileName,
                                            Description = "Excel Dosyası Boş"
                                        };
                                    }
                                    else
                                    {
                                        // Verileri işleme
                                        DataSet result = reader.AsDataSet();
                                        DataTable table = result.Tables[0];

                                        var startIndex = 2; //kaçıncı indexten başlıyorsa o index değeri verilir. (Başlıktan sonraki index)
                                        for (int i = startIndex; i < table.Rows.Count; i++)
                                        {
                                            string kupe = table.Rows[i][0].ToString(); // kupeNo
                                            string padok = table.Rows[i][1].ToString(); // Padok

                                            if (kupe != null && padok != null)
                                            {
                                                // Excelden alınan datalar model'e dolduruluyor.
                                                modelList.Add(new SuruHareketleri
                                                {
                                                    CreatedDate = DateTime.Now,
                                                    kupeNo = kupe,
                                                    Padok = padok,

                                                });
                                            }

                                        }
                                        foreach (var model in modelList)
                                        {
                                            suruDal.Add(model); // Veritabanına ekleme işlemi 
                                        }
                                    }
                                }
                            }
                            string newFileName = fileName.Replace("YemMerkeziListe ", "X"); // Dosya işlendikten sonra adını değiştiriyoruz. bir daha işlememek için
                            string newFilePath = Path.Combine(Path.GetDirectoryName(filePath), newFileName);
                            File.Copy(filePath, newFilePath);
                            File.Delete(filePath); // Eski dosya siliniyor
                        }
                    }
                    catch (Exception ex)
                    {
                        return new ResultMessageModel()
                        {
                            Description = ex.Message,
                            FileName = fileName
                        };
                    }
                }
                return new ResultMessageModel()
                {
                    FileName = string.Empty,
                    Description = "Success",
                };
            }
            else
            {
                return new ResultMessageModel()
                {
                    FileName = etapKlasoruYolu,
                    Description = "Dosya Yolu Hatalı"
                };
            }
        }
    }
}
