using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireApp.Entities.Concrete
{
    public class SuruHareketleri
    {
        public int Id { get; set; }
        public string kupeNo{ get; set; }
        public string Padok{ get; set; }
        public DateTime CreatedDate{ get; set; }
    }
}
