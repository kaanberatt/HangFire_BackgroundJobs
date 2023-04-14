using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireApp.Entities.Concrete
{
    public class IntegrationLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public bool isActive{ get; set; }
        public DateTime Date { get; set; }
    }
}
