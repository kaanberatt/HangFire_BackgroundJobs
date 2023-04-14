using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireApp.Service.Settings
{
    public class RecurringJobSettings
    {
        public RepetitiveJob SuruHareketleriJob { get; set; }
        public RepetitiveJob DeleteExcelFilesJob { get; set; }
        public RepetitiveJob HayvanHareketleriJob { get; set; }
    }
}
