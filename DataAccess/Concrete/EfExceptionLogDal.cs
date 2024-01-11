using Core.DataAccess.Concrete;
using DataAccess.Abstract;
using Entities.Concrete;

namespace DataAccess.Concrete
{
    public class EfExceptionLogDal : EfEntityRepositoryBase<ExceptionLog, DatabaseContext>, IExceptionLogDal
    {
    }
}
