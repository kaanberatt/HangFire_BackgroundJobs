using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireApp.Service.Jobs.Interfaces
{
    public interface IHayvanHareketleriJob
    {
        [AutomaticRetry(Attempts = 5)]
        [JobDisplayName("Hayvan Hareketleri Job")]
        [Queue("default")]
        void Run();
    }
}
