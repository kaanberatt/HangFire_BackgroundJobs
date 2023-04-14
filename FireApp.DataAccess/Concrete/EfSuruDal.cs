using FireApp.DataAccess.Abstract;
using FireApp.DataAccess.Context;
using FireApp.DataAccess.GenericRepository;
using FireApp.Entities.Concrete;

namespace FireApp.DataAccess.Concrete
{
    public class EfSuruDal : EfEntityRepositoryBaseAsync<SuruHareketleri,FireAppDbContext>, ISuruDal
    {
    }
}
