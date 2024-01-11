using Core.Entites.Concrete;

namespace Core.Utulities.Security.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);

    }
}
