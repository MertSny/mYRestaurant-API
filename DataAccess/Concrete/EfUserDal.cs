using Core.DataAccess.Concrete;
using Core.Entites.Concrete;
using DataAccess.Abstract;

namespace DataAccess.Concrete
{
    public class EfUserDal : EfEntityRepositoryBase<User, DatabaseContext>, IUserDal
    {
    }
}
