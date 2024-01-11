using Core.DataAccess.Abstract;
using Core.Entites.Concrete;

namespace DataAccess.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
    }
}
