using Core.Helpers;
using Core.Model.SearchRequests;
using Core.Utulities.Result;
using Entities.DTO;
using static Core.Enums.OrderByEnums;

namespace Business.Abstract
{
    public interface IUserOperationClaimService
    {
        Task<IDataResult<UserOperationClaimDto>> AddAsync(UserOperationClaimDto dto, UserContext currentUser);
        Task<IDataResult<UserOperationClaimDto>> GetById(int id);
        Task<IResult> Delete(int id, UserContext currentUser);
        Task<IResult> UpdateAsync(UserOperationClaimDto dto, UserContext currentUser);
        Task<IDataResult<IEnumerable<UserOperationClaimDto>>> Search(UserOperationClaimSearchRequest dto);
        Task<IDataResult<PagedResult<UserOperationClaimDto>>> SearchWithPagination(FilterRequest<DefaultOrderBy, UserOperationClaimSearchRequest> request);
    }
}
