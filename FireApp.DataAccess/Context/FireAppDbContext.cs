using FireApp.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace FireApp.DataAccess.Context
{
    public class FireAppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string ConnectService = "Server=DEVLNB01; Database=TelosJobDB; Trusted_Connection=True;";
            optionsBuilder.UseSqlServer(ConnectService);
        }
        public DbSet<SuruHareketleri> SuruHareketleri{ get; set; }

        public DbSet<IntegrationLog> IntegrationLog{ get; set; }
    }
}
