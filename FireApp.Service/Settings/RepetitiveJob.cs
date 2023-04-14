using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireApp.Service.Settings
{
    public class RepetitiveJob : Job
    {
        public string IntervalPattern { get; set; } // Tekrarlanan job'ların interval değeri var.

    }
}
