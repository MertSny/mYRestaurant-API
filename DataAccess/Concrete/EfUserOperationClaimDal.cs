using Core.DataAccess.Concrete;
using Core.Entites.Concrete;
using DataAccess.Abstract;

namespace DataAccess.Concrete
{
    public class EfUserOperationClaimDal : EfEntityRepositoryBase<UserOperationClaim, DatabaseContext>,IUserOperationClaimDal
    {
    }
}
