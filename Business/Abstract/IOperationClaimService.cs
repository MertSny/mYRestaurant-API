using Core.Helpers;
using Core.Model.SearchRequests;
using Core.Utulities.Result;
using Entities.DTO;
using static Core.Enums.OrderByEnums;

namespace Business.Abstract
{
    public interface IOperationClaimService
    {
        Task<IDataResult<OperationClaimDto>> AddAsync(OperationClaimDto dto, UserContext currentUser);
        Task<IDataResult<OperationClaimDto>> GetById(int id);
        Task<IResult> Delete(int id, UserContext currentUser);
        Task<IResult> UpdateAsync(OperationClaimDto dto, UserContext currentUser);
        Task<IDataResult<IEnumerable<OperationClaimDto>>> Search(OperationClaimSearchRequest dto);
        Task<IDataResult<PagedResult<OperationClaimDto>>> SearchWithPagination(FilterRequest<DefaultOrderBy, OperationClaimSearchRequest> request);
    }
}
