using FireApp.DataAccess.Abstract;
using FireApp.DataAccess.Context;
using FireApp.DataAccess.GenericRepository;
using FireApp.Entities.Concrete;


namespace FireApp.DataAccess.Concrete
{
    public class EfIntegrationLogDal : EfEntityRepositoryBaseAsync<IntegrationLog, FireAppDbContext>, IIntergrationLogDal
    {
    }
}
