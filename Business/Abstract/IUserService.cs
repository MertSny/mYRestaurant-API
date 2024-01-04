using Core.Helpers;
using Core.Model.SearchRequests;
using Core.Utulities.Result;
using Entities.DTO;
using static Core.Enums.OrderByEnums;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<UserDto>> AddAsync(UserDto dto, UserContext currentUser);
        Task<IDataResult<UserDto>> GetById(int id);
        Task<IResult> Delete(int id, UserContext currentUser);
        Task<IResult> UpdateAsync(UserDto dto, UserContext currentUser);
        Task<IDataResult<IEnumerable<UserDto>>> Search(UserSearchRequest dto);
        Task<IDataResult<PagedResult<UserDto>>> SearchWithPagination(FilterRequest<DefaultOrderBy, UserSearchRequest> request);
    }
}
